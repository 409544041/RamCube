using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Profiling;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace ProjectDawn.LocalAvoidance.Demo
{
    [BurstCompile]
    public struct SpatialPartitioningJob : IJobParallelForTransform
    {
        [WriteOnly]
        public NativeMultiHashMap<int, int>.ParallelWriter HashMap;
        public float CellRadius;

        public void Execute(int i, TransformAccess transformAccess)
        {
            var hash = (int)math.hash(new int2(
                (int)(transformAccess.position.x / CellRadius),
                (int)(transformAccess.position.z / CellRadius)));
            HashMap.Add(hash, i);
        }
    }
    
    [BurstCompile]
    public struct LocalAvoidanceJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<Agent> Agents;
        [WriteOnly]
        public NativeArray<float3> Velocities;
        public float SonarForwardVelocityScaler;
        public float SonarBackVelocityScaler;
        public float SonarCutBackVisionAngle;

        [ReadOnly]
        public NativeMultiHashMap<int, int> HashMap;
        public float CellRadius;
        public float DeltaTime;

        public struct Agent
        {
            public float3 Position;
            public float3 Velocity;
            public float3 Destination;
            public float Radius;
            public float Speed;
            public float SonarRadius;
        }

        public void Execute(int i)
        {
            Velocities[i] = LocalAvoidance(i);
        }

        float3 LocalAvoidance(int current)
        {
            var agent = Agents[current];

            var desiredDirection = math.normalizesafe(agent.Destination - agent.Position);
            desiredDirection.y = 0;
            if (math.lengthsq(desiredDirection) == 0)
                return desiredDirection;

            var sonarRadius = math.min(agent.SonarRadius, math.distance(agent.Destination, agent.Position));

            var dynamics = new SonarDynamics
            {
                Velocity = agent.Velocity,
                ForwardVelocityScaler = SonarForwardVelocityScaler,
                BackVelocityScaler = SonarBackVelocityScaler,
            };
            var sonar = new SonarAvoidance(agent.Position, desiredDirection, new float3(0, 1, 0), agent.Radius, sonarRadius, dynamics);

            var nearbyAgents = Agents;

            // Without spatial partitioning
            /*for (int i = 0; i < Agents.Length; ++i)
            {
                // Skip itself
                if (i == current)
                    continue;

                var nearbyAgent = nearbyAgents[i];
                sonar.InsertObstacle(nearbyAgent.Position, nearbyAgent.Velocity, nearbyAgent.Radius);
            }*/

            float2 boxMin = (agent.Position.xz - agent.SonarRadius);
            float2 boxMax = (agent.Position.xz + agent.SonarRadius);

            // Find how many chunks does box overlap
            int2 min = (int2) (boxMin / CellRadius);
            int2 max = (int2) (boxMax / CellRadius) + 2;

            for (int i = min.x; i < max.x; ++i)
            {
                for (int j = min.y; j < max.y; ++j)
                {
                    int key = GetCellHash(i, j);

                    // Find all entities in the bucket
                    if (HashMap.TryGetFirstValue(key, out var index, out var iterator))
                    {
                        do
                        {
                            // Skip itself
                            if (index == current)
                                continue;

                            var nearbyAgent = nearbyAgents[index];
                            sonar.InsertObstacle(nearbyAgent.Position, nearbyAgent.Velocity, nearbyAgent.Radius);
                        }
                        while (HashMap.TryGetNextValue(out index, ref iterator));
                    }
                }
            }

            // Add blocker behind the velocity
            // This will prevent situations where agent has on right and left equally good paths
            sonar.InsertObstacle(math.normalizesafe(-agent.Velocity), math.radians(SonarCutBackVisionAngle));

            sonar.FindClosestDirection(out desiredDirection);

            sonar.Dispose();

            var impulse = desiredDirection * agent.Speed;

            return impulse;
        }

        static int GetCellHash(int x, int y)
        {
            var hash = (int)math.hash(new int2(x, y));
            return hash;
        }
    }

    [BurstCompile]
    public struct VelocityJob : IJobParallelForTransform
    {
        [ReadOnly]
        public NativeArray<float3> Velocities;

        public NativeArray<Agent> Agents;
        public float DeltaTime;

        public struct Agent
        {
            public float3 Velocity;
            public float Acceleration;
            public float TurnSpeed;
            public bool IsStopped;
        }

        public void Execute(int i, TransformAccess transformAccess)
        {
            var agent = Agents[i];

            var velocity = math.lerp(agent.Velocity, Velocities[i], DeltaTime * agent.Acceleration);
            agent.Velocity = velocity;

            if (math.length(velocity) != 0)
            {
                var offset = velocity * DeltaTime;

                // Avoid over-steping destination
                //var distance = math.distance(agent.Destination, agent.Position);
                //offset = ForceLength(offset, distance);

                transformAccess.position += (Vector3) offset;

                var rotation = quaternion.LookRotation(math.normalizesafe(velocity), new float3(0, 1, 0));
                transformAccess.rotation = math.slerp(transformAccess.rotation, rotation, DeltaTime * agent.TurnSpeed);
            }
        }
    }

    /// <summary>
    /// System that executs local avoidance logic for <see cref="Agent"/>.
    /// Uses jobs system.
    /// </summary>
    [DefaultExecutionOrder(50)]
    public class JobifiedAgentSystem : System
    {
        [Range(1, 10)]
        public float SonarRadius = 6;
        [Range(0, 1)]
        public float SonarForwardVelocityScaler = 1;
        [Range(0, 1)]
        public float SonarBackVelocityScaler = 1;
        [Range(0, 180)]
        public float SonarCutBackVisionAngle = 135f;
        public float AgentAcceleration = 8;
        [Range(1, 100)]
        public int AgentPerJob = 5;
        public float SpatialPartitioningCellRadius = 5;

        void FixedUpdate()
        {
            var agents = Query<Agent>();

            // Early out
            if (agents.Length == 0)
                return;

            Profiler.BeginSample("ScheduleSpatialPartitioningJob");
            var hashMap = new NativeMultiHashMap<int, int>(agents.Length, Allocator.TempJob);

            var transforms = new TransformAccessArray(agents.Length);
            for (int i = 0; i < agents.Length; ++i)
            {
                var agent = agents[i];
                transforms.Add(agent.transform);
            }

            var spatialPartitioningJob = new SpatialPartitioningJob
            {
                HashMap = hashMap.AsParallelWriter(),
                CellRadius = SpatialPartitioningCellRadius,
            };
            var handle = spatialPartitioningJob.Schedule(transforms);
            Profiler.EndSample();

            Profiler.BeginSample("ScheduleLocalAvoidanceJob");
            var localAvoidanceAgents = new NativeArray<LocalAvoidanceJob.Agent>(agents.Length, Allocator.TempJob);
            for (int i = 0; i < agents.Length; ++i)
            {
                var agent = agents[i];
                localAvoidanceAgents[i] = new LocalAvoidanceJob.Agent
                {
                    Position = agent.Position,
                    Velocity = agent.Velocity,
                    Speed = agent.Speed,
                    Radius = agent.Radius,
                    Destination = agent.Destination,
                    SonarRadius = SonarRadius,
                };
            }

            var velocities = new NativeArray<float3>(agents.Length, Allocator.TempJob, NativeArrayOptions.ClearMemory);

            var job = new LocalAvoidanceJob
            {
                Agents = localAvoidanceAgents,
                SonarForwardVelocityScaler = SonarForwardVelocityScaler,
                SonarBackVelocityScaler = SonarBackVelocityScaler,
                SonarCutBackVisionAngle = SonarCutBackVisionAngle,
                Velocities = velocities,
                DeltaTime = Time.deltaTime,

                HashMap = hashMap,
                CellRadius = SpatialPartitioningCellRadius,
            };

            handle = job.Schedule(localAvoidanceAgents.Length, AgentPerJob, handle);
            Profiler.EndSample();

            Profiler.BeginSample("ScheduleVelocityJob");
            var velocityAgents = new NativeArray<VelocityJob.Agent>(agents.Length, Allocator.TempJob);
            for (int i = 0; i < agents.Length; ++i)
            {
                var agent = agents[i];
                velocityAgents[i] = new VelocityJob.Agent
                {
                    Velocity = agent.Velocity,
                    Acceleration = AgentAcceleration,
                    IsStopped = agent.IsStopped,
                    TurnSpeed = agent.TurnSpeed,
                };
            }

            var velocityJob = new VelocityJob
            {
                Agents = velocityAgents,
                Velocities = velocities,
                DeltaTime = Time.deltaTime,
            };

            handle = velocityJob.Schedule(transforms, handle);
            Profiler.EndSample();

            handle.Complete();

            for (int i = 0; i < agents.Length; ++i)
            {
                var agent = agents[i];
                var velocityAgent = velocityAgents[i];
                agent.Velocity = velocityAgent.Velocity;
            }

            localAvoidanceAgents.Dispose();
            velocities.Dispose();
            velocityAgents.Dispose();
            transforms.Dispose();

            hashMap.Dispose();
        }
    }
}
