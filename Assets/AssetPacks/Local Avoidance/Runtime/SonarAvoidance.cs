// @ 2022 Lukas Chodosevicius

using UnityEngine;
using UnityEngine.Assertions;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;
using System;
using System.Runtime.CompilerServices;

namespace ProjectDawn.LocalAvoidance
{
    /// <summary>
    /// This structure can be used for constructing local objet avoidance structure and finding the closest direction for it.
    /// It is composed of three main methods: Constructor, InsertObstacle and FindClosestDirection.
    /// </summary>
    [BurstCompile]
    public struct SonarAvoidance : IDisposable
    {
        const float s_Angle = math.PI;

        float3 m_Position;
        quaternion m_Rotation;
        SonarDynamics m_Dynamics;
        float m_InnerRadius;
        float m_OuterRadius;
        NativeList<SonarNode> m_Nodes;

        /// <summary>
        /// Root node. Always starts at zero handle.
        /// </summary>
        SonarNodeHandle Root => new SonarNodeHandle();
        /// <summary>
        /// True if structure is created.
        /// </summary>
        public bool IsCreated => m_Nodes.IsCreated;

        /// <summary>
        /// Constructs copy of sonar avoidance. No memory is shared between copy and original.
        /// </summary>
        /// <param name="other">Copies from</param>
        /// <param name="allocator">Allocator type</param>
        public SonarAvoidance(in SonarAvoidance other, Allocator allocator = Allocator.Temp)
        {
            if (!other.IsCreated)
                throw new ArgumentException("Can not copy from structure that is not created.", "other");

            m_Position = other.m_Position;
            m_Rotation = other.m_Rotation;
            m_InnerRadius = other.m_InnerRadius;
            m_OuterRadius = other.m_OuterRadius;
            m_Dynamics = other.m_Dynamics;

            // Make a copy of nodes
            // Uses unsafe context for faster copy API
            unsafe
            {
                var length = other.m_Nodes.Length;
                m_Nodes = new NativeList<SonarNode>(length, allocator);
                m_Nodes.ResizeUninitialized(length);
                UnsafeUtility.MemCpy(m_Nodes.GetUnsafePtr(), other.m_Nodes.GetUnsafePtr(), sizeof(SonarNode) * length);
            }
        }

        /// <summary>
        /// Constructs sonar avoidance using position, direction and radius.
        /// </summary>
        /// <param name="position">Position of sonar</param>
        /// <param name="direction">Direction of sonar. Note this is forward direction on x axis, not z like LookRotation uses</param>
        /// <param name="up">Up direction</param>
        /// <param name="innerRadius">Minimum radius from which sonar will tracks obstacles and also used for path size</param>
        /// <param name="outerRadius">Maximum radius from which sonar will tracks obstacles</param>
        /// <param name="dynamics">Settings for avoiding moving objects</param>
        /// <param name="allocator">Allocator type</param>
        public SonarAvoidance(float3 position, float3 direction, float3 up, float innerRadius, float outerRadius, SonarDynamics dynamics, Allocator allocator = Allocator.Temp)
            : this(position, SonarAvoidance.DirectionToRotation(direction, up), innerRadius, outerRadius, dynamics, allocator) {}

        /// <summary>
        /// Constructs sonar avoidance using position, direction and radius.
        /// </summary>
        /// <param name="position">Position of sonar</param>
        /// <param name="rotation">Rotation of sonar</param>
        /// <param name="innerRadius">Minimum radius from which sonar will tracks obstacles and also used for path size</param>
        /// <param name="outerRadius">Maximum radius from which sonar will tracks obstacles</param>
        /// <param name="dynamics">Settings for avoiding moving objects</param>
        /// <param name="allocator">Allocator type</param>
        public SonarAvoidance(float3 position, quaternion rotation, float innerRadius, float outerRadius, SonarDynamics dynamics, Allocator allocator = Allocator.Temp)
        {
            if (innerRadius < 0)
                throw new ArgumentException("Radius must be non negative", "innerRadius");
            if (outerRadius <= 0)
                throw new ArgumentException("Radius must be greater than zero", "outerRadius");
            if (math.any(!math.isfinite(rotation.value)))
                throw new ArgumentException($"Rotation cannot be zero or Infinite/NaN. ({rotation.value})", "rotation");

            m_Position = position;
            m_Rotation = rotation;
            m_InnerRadius = innerRadius;
            m_OuterRadius = outerRadius;
            m_Dynamics = dynamics;
            m_Nodes = new NativeList<SonarNode>(16, allocator);

            // Create Root node with childs left and right
            CreateNode(new Line(-s_Angle, s_Angle));
            var left = CreateNode(new Line(-s_Angle, 0));
            var right = CreateNode(new Line(0, s_Angle));
            m_Nodes[Root] = new SonarNode
            {
                Line = new Line(-s_Angle, s_Angle),
                Left = left,
                Right = right,
            };
        }

        /// <summary>
        /// Inserts radius obstacle into sonar.
        /// </summary>
        /// <param name="direction">Direction of obstacle from sonar</param>
        /// <param name="radius">Radius of obstacle</param>
        /// <returns> True if obstacle was added successfully</returns>
        public bool InsertObstacle(float3 direction, float radius)
        {
            if (!IsCreated)
                throw new Exception("SonarAvoidance is not initialized. It can happen if was created with argumentless contructor or disposed.");

            var directionLS = ToLocalSpace(direction);
            var angle = DirectionLSToAngle(directionLS);

            var radiusHalf = radius * 0.5f;
            var angleRight = angle - radiusHalf;
            var angleLeft = angle + radiusHalf;

            angleRight = ConvertAngleToMaxiumOfPI(angleRight);
            angleLeft = ConvertAngleToMaxiumOfPI(angleLeft);

            // In different hemisphere, we can simply skip it for non full sphere vision
            if (angleRight > angleLeft)
            {
                InsertObstacle(m_Nodes[Root].Right, new Line(angleRight, math.PI));
                InsertObstacle(m_Nodes[Root].Left, new Line(-math.PI, angleLeft));
                return true;
            }

            InsertObstacle(Root, new Line(angleRight, angleLeft));
            return true;
        }

        /// <summary>
        /// Inserts sphere obstacle into sonar.
        /// </summary>
        /// <param name="obstaclePosition">Position of obstacle</param>
        /// <param name="obstacleVelocity">Velocity of obstacle (Zero can be used for non moving obstacle)</param>
        /// <param name="obstacleRadius">Radius of obstacle</param>
        /// <returns> True if obstacle was added successfully</returns>
        public bool InsertObstacle(float3 obstaclePosition, float3 obstacleVelocity, float obstacleRadius)
        {
            if (!IsCreated)
                throw new Exception("SonarAvoidance is not initialized. It can happen if was created with argumentless contructor or disposed.");

            if (math.distance(m_Position, obstaclePosition) > obstacleRadius + m_OuterRadius)
                return false;

            var towardsWS = obstaclePosition - m_Position;
            var towardsLS = ToLocalSpace(towardsWS);

            var directionLS = math.normalizesafe(towardsLS);

            // Here we find tangent line to circle https://en.wikipedia.org/wiki/Tangent_lines_to_circles
            // Tangent line to a circle is a line that touches the circle at exactly one point, never entering the circle's interior
            var opp = obstacleRadius + m_InnerRadius;
            var hyp = math.max(math.length(towardsLS), obstacleRadius);
            var tangentLineAngle = math.asin(math.clamp(opp / hyp, -1, 1));

            // Convert direction to angles
            var angle = DirectionLSToAngle(directionLS);
            var directionRightLS = AngleToDirectionLS(angle - tangentLineAngle);
            var directionLeftLS = AngleToDirectionLS(angle + tangentLineAngle);

            // Length of this line
            var tangetLineLength = math.cos(tangentLineAngle) * hyp;

            // Closer circle gets to the origin the more tangent line shrinks
            // In order to avoid this we force distance to minimum small value
            tangetLineLength = math.max(tangetLineLength, 0.001f);

            var towardsRight = directionRightLS * tangetLineLength;
            var towardsLeft = directionLeftLS * tangetLineLength;

            // Find the time will take to reach from sonar center to contact point
            var sonarVelocityLength = math.length(m_Dynamics.Velocity);
            if (sonarVelocityLength == 0)
                return false;
            var time = tangetLineLength / sonarVelocityLength;

            var up = new float3(0, 1, 0);
            var right = math.cross(up, directionLS);
            var left = -right;

            // TODO: This needs to be changed to more accurate solution which does not require settings
            // Idea of this code is to stretch the obstacle towards its velocity
            // This way sonar will be less likely to take direction that will intersect with obstacles velocity
            var obstacleVelocityLS = ToLocalSpace(obstacleVelocity);
            var velocityOffset = (obstacleVelocityLS * time);
            var isLeftForward = math.dot(obstacleVelocityLS, left) > 0;
            if (isLeftForward)
            {
                towardsLeft += velocityOffset * m_Dynamics.ForwardVelocityScaler;
                towardsRight += velocityOffset * m_Dynamics.BackVelocityScaler;
            }
            else
            {
                towardsLeft += velocityOffset * m_Dynamics.BackVelocityScaler;
                towardsRight += velocityOffset * m_Dynamics.ForwardVelocityScaler;
            }
            Assert.IsFalse(math.dot(obstacleVelocityLS, left) > 0 && math.dot(obstacleVelocityLS, right) > 0);

            // Convert righ and left directions to angles
            var directionRight = math.normalizesafe(towardsRight);
            var angleRight = DirectionLSToAngle(directionRight);
            var directionLeft = math.normalizesafe(towardsLeft);
            var angleLeft = DirectionLSToAngle(directionLeft);

            Assert.IsFalse(math.isnan(angleRight));
            Assert.IsFalse(math.isnan(angleLeft));

            // In different hemisphere, we can simply skip it for non full sphere vision
            if (angleRight > angleLeft)
            {
                InsertObstacle(m_Nodes[Root].Right, new Line(angleRight, math.PI));
                InsertObstacle(m_Nodes[Root].Left, new Line(-math.PI, angleLeft));
                return true;
            }

            InsertObstacle(Root, new Line(angleRight, angleLeft));
            return true;
        }

        /// <summary>
        /// Finds closest desired direction that is not obstructed by obstacle.
        /// </summary>
        /// <param name="direction">Closest direction found</param>
        /// <returns> True if direction was found</returns>
        public bool FindClosestDirection(out float3 direction)
        {
            if (!IsCreated)
                throw new Exception("SonarAvoidance is not initialized. It can happen if was created with argumentless contructor or disposed.");

            // Find closest angle in left side nodes
            var successLeft = false;
            var angleLeft = float.MaxValue;
            FindClosestAngle(m_Nodes[Root].Left, ref angleLeft, ref successLeft);

            // Find closest angle in right side nodes
            var successRight = false;
            var angleRight = float.MaxValue;
            FindClosestAngle(m_Nodes[Root].Right, ref angleRight, ref successRight);

            if (successLeft && successRight)
            {
                if (math.abs(angleLeft) < math.abs(angleRight))
                {
                    direction = ToWorldSpace(AngleToDirectionLS(angleLeft));
                    return true;
                }
                else
                {
                    direction = ToWorldSpace(AngleToDirectionLS(angleRight));
                    return true;
                }
            }
            else if (successLeft)
            {
                direction = ToWorldSpace(AngleToDirectionLS(angleLeft));
                return true;
            }
            else if (successRight)
            {
                direction = ToWorldSpace(AngleToDirectionLS(angleRight));
                return true;
            }

            direction = float3.zero;
            return false;
        }

        /// <summary>
        /// Dispose implementation.
        /// </summary>
        public void Dispose()
        {
            m_Nodes.Dispose();
        }

        void InsertObstacle(SonarNodeHandle handle, Line line)
        {
            Assert.AreNotEqual(handle, SonarNodeHandle.Null);

            SonarNode node = m_Nodes[handle];

            var nodeLine = node.Line;
            if (line.Length == 0)
                return;

            if (Line.CutLine(nodeLine, line, out var result))
            {
                if (node.IsLeaf)
                {
                    switch (result.SegmentCount)
                    {
                        case 2:
                            node.Left = CreateNode(result.Segment0);
                            node.Right = CreateNode(result.Segment1);
                            m_Nodes[handle] = node;
                            break;

                        case 1:
                        case 0:
                            node.Line = result.Segment0;
                            m_Nodes[handle] = node;
                            break;

                        default:
                            throw new NotImplementedException("There can only be 0, 1 or 2 segments.");
                    }
                }
                else
                {
                    InsertObstacle(node.Right, line);
                    InsertObstacle(node.Left, line);
                }
            }
        }

        SonarNodeHandle CreateNode(Line line)
        {
            m_Nodes.Add(new SonarNode(line));
            return new SonarNodeHandle { Index = m_Nodes.Length - 1 };
        }

        void FindClosestAngle(SonarNodeHandle handle, ref float angle, ref bool success)
        {
            var node = m_Nodes[handle];
            var line = node.Line;

            if (line.Length <= 0)
                return;

            if (node.IsLeaf)
            {
                if (math.abs(line.From) < math.abs(angle))
                {
                    angle = line.From;
                    success = true;
                }
                if (math.abs(line.To) < math.abs(angle))
                {
                    angle = line.To;
                    success = true;
                }
            }
            else
            {
                FindClosestAngle(node.Left, ref angle, ref success);
                FindClosestAngle(node.Right, ref angle, ref success);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        float3 ToLocalSpace(float3 value)
        {
            return math.mul(math.conjugate(m_Rotation), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        float3 ToWorldSpace(float3 value)
        {
            return math.mul(m_Rotation, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float3 AngleToDirectionLS(float angle)
        {
            float3 direction = new float3(math.cos(angle), 0, math.sin(angle));
            return direction;
        }

        /// <summary>
        /// This is almost same as Quaternion.LookRotation just uses forward as x axis instead of z.
        /// </summary>
        /// <param name="forward">Forward direction on x axis</param>
        /// <param name="up">Up direction</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static quaternion DirectionToRotation(float3 forward, float3 up)
        {
            float3 t = math.normalize(math.cross(up, forward));
            return new quaternion(new float3x3(forward, math.cross(t, forward), t));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float DirectionLSToAngle(float3 direction)
        {
            var angle = math.atan2(direction.z, direction.x);
            return angle;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float ConvertAngleToMaxiumOfPI(float angle)
        {
            if (angle > math.PI)
            {
                return angle = angle-(2*math.PI);
            }
            else if (angle < -math.PI)
            {
                return angle+(2*math.PI);
            }
            return angle;
        }

        /// <summary>
        /// Draws closest desired direction that is not obstructed by obstacle. Must be called inside <see cref="MonoBehaviour.OnGizmos"/> and only works in Editor.
        /// </summary>
        public void DrawClosestDirection()
        {
#if UNITY_EDITOR
            if (!IsCreated)
                throw new Exception("SonarAvoidance is not initialized. It can happen if was created with argumentless contructor or disposed.");

            if (!FindClosestDirection(out var direction))
                return;

            float3 position = m_Position;

            float3 up = new float3(0, 1, 0);
            up = math.mul(m_Rotation, up);

            float3 r = math.cross(up, direction);
            float3 l = -math.cross(up, direction);

            float3 rr = r * m_InnerRadius;
            float3 ll = l * m_InnerRadius;

            Vector3[] vertices = new Vector3[4];
            vertices[0] = position + rr;
            vertices[1] = position + direction * m_OuterRadius + rr;
            vertices[2] = position + direction * m_OuterRadius + ll;
            vertices[3] = position + ll;

            UnityEditor.Handles.color = new Color(0, 0, 1, 0.3f);
            UnityEditor.Handles.DrawAAConvexPolygon(vertices);
#endif
        }

        /// <summary>
        /// Draws sonar that is not obstructed by obstacle. Must be called inside <see cref="MonoBehaviour.OnGizmos"/> and only works in Editor.
        /// </summary>
        public void DrawSonar()
        {
            if (!IsCreated)
                throw new Exception("SonarAvoidance is not initialized. It can happen if was created with argumentless contructor or disposed.");

            DrawSonar(Root, m_Position, m_Rotation);
        }

        void DrawSonar(SonarNodeHandle handle, float3 position, quaternion rotation)
        {
#if UNITY_EDITOR
            var node = m_Nodes[handle];

            if (node.IsLeaf)
            {
                var line = node.Line;

                if (line.Length == 0)
                    return;

                float3 directionFromWS = ToWorldSpace(AngleToDirectionLS(line.From));
                float3 directionToWS = ToWorldSpace(AngleToDirectionLS(line.To));

                float3 up = Vector3.up;
                up = math.mul(rotation, up);

                UnityEditor.Handles.color = new Color(0, 1, 0, 0.3f);
                UnityEditor.Handles.DrawSolidArc(position, up, directionToWS, math.degrees(line.Length), m_OuterRadius);

                UnityEditor.Handles.color = new Color(1, 0, 0, 0.3f);
                UnityEditor.Handles.DrawSolidArc(position, up, directionToWS, math.degrees(line.Length), m_InnerRadius);
            }
            else
            {
                DrawSonar(node.Left, position, rotation);
                DrawSonar(node.Right, position, rotation);
            }
#endif
        }
    }
}
