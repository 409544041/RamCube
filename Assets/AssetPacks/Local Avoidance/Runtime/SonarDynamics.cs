// @ 2022 Lukas Chodosevicius

using Unity.Mathematics;

namespace ProjectDawn.LocalAvoidance
{
    /// <summary>
    /// Settings of <see cref="SonarAvoidance"/> for avoiding moving objects.
    /// </summary>
    public struct SonarDynamics
    {
        /// <summary>
        /// Velocity of sonar volume.
        /// </summary>
        public float3 Velocity;
        /// <summary>
        /// Controls how much obstacle front is stretched towards obstacle velocity direction.
        /// </summary>
        public float ForwardVelocityScaler;
        /// <summary>
        /// Controls how much obstacle back is stretched towards obstacle velocity direction.
        /// </summary>
        public float BackVelocityScaler;

        /// <summary>
        /// Constructs with default settings.
        /// </summary>
        /// <param name="velocity">Velocity of sonar volume</param>
        public SonarDynamics(float3 velocity)
        {
            Velocity = velocity;
            ForwardVelocityScaler = 1;
            BackVelocityScaler = 0.5f;
        }
    }
}
