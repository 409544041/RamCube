using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;

namespace ProjectDawn.LocalAvoidance.Demo
{
    /// <summary>
    /// System that executs local avoidance logic for <see cref="Agent"/>.
    /// </summary>
    [DefaultExecutionOrder(50)]
    public class AgentSystem : System
    {
        [Range(1, 10)]
        public float SonarRadius = 6;
        [Range(0, 1)]
        public float SonarForwardVelocityScaler = 1;
        [Range(0, 1)]
        public float SonarBackVelocityScaler = 1;
        [Range(0, 180)]
        public float SonarCutBackVisionAngle = 135f;
        public bool ChangeTransform = true;
        public bool Is3D = true;
        public float AgentAcceleration = 8;

        void FixedUpdate()
        {
            var agents = Query<Agent>();
            if (agents.Length == 0)
                return;

            foreach (var agent in agents)
            {
                float3 impulse = 0;
                if (!agent.IsStopped && agent.Speed > 0)
                {
                    float3 desiredDirection = math.normalizesafe(agent.Destination - agent.Position);
                    impulse += GetAvoid(agent, agents, desiredDirection);
                }

                var velocity = math.lerp(agent.Velocity, impulse, Time.deltaTime * AgentAcceleration);
                agent.Velocity = velocity;

                if (ChangeTransform && math.lengthsq(agent.Velocity) != 0)
                {
                    var offset = agent.Velocity * Time.deltaTime;

                    // Avoid over-steping destination
                    var distance = math.distance(agent.Destination, agent.Position);
                    offset = ForceLength(offset, distance);

                    agent.transform.position += (Vector3)offset;

                    if (Is3D)
                    {
                        var rotation = quaternion.LookRotation(math.normalizesafe(agent.Velocity), new float3(0, 1, 0));
                        agent.transform.rotation = math.slerp(transform.rotation, rotation, Time.deltaTime * agent.TurnSpeed);
                    }
                }
            }
        }

        float3 GetAvoid(Agent agent, Agent[] nearbyAgents, float3 desiredDirection)
        {
            if (math.lengthsq(desiredDirection) == 0)
                return float3.zero;

            // Destination should not be farther than the vision
            var sonarRadius = math.min(SonarRadius, math.distance(agent.Destination, agent.Position));

            UnityEngine.Profiling.Profiler.BeginSample("SonarAvoidance");
            var dynamics = new SonarDynamics
            {
                Velocity = agent.Velocity,
                ForwardVelocityScaler = SonarForwardVelocityScaler,
                BackVelocityScaler = SonarBackVelocityScaler,
            };

            var up = Is3D ? new float3(0, 1, 0) : new float3(0, 0, -1);
            var sonar = new SonarAvoidance(agent.Position, desiredDirection, up, agent.Radius, sonarRadius, dynamics);

            foreach (var nearbyAgent in nearbyAgents)
            {
                // Skip itself
                if (nearbyAgent == agent)
                    continue;

                sonar.InsertObstacle(nearbyAgent.Position, nearbyAgent.Velocity, nearbyAgent.Radius);
            }

            // Add blocker behind the velocity
            // This will prevent situations where agent has on right and left equally good paths
            sonar.InsertObstacle(math.normalizesafe(-agent.Velocity), math.radians(SonarCutBackVisionAngle));

            sonar.FindClosestDirection(out desiredDirection);

            // Update debug component for drawing
            if (agent.gameObject.TryGetComponent(out AgentDebug agentDebug))
            {
                if (agentDebug.Vision.IsCreated)
                    agentDebug.Vision.Dispose();

                agentDebug.Vision = new SonarAvoidance(sonar, Allocator.Persistent);
            }

            sonar.Dispose();
            UnityEngine.Profiling.Profiler.EndSample();

            return desiredDirection * agent.Speed;
        }

        float3 ForceLength(float3 value, float length)
        {
            var valueLength = math.length(value);
            return valueLength > length ? value / valueLength * length : value;
        }
    }
}
