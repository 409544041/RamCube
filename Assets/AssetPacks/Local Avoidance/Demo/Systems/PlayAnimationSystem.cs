using UnityEngine;
using Unity.Mathematics;

namespace ProjectDawn.LocalAvoidance.Demo
{
    [DefaultExecutionOrder(60)]
    public class PlayAnimationSystem : System
    {
        public float AnimationSpeed = 0.4f;
        void Update()
        {
            foreach (var playAnimation in Query<PlayAnimation>())
            {
                if (!playAnimation.Target)
                    continue;

                if (!playAnimation.TryGetComponent(out BoidsAgent agent))
                    continue;

                if (!playAnimation.Target.TryGetComponent(out Animator animator))
                    continue;

                var speed = math.length(agent.Velocity);
                animator.SetFloat("Speed", speed);
                animator.speed = speed > 0.3f ? speed * AnimationSpeed : 1f;
            }
        }
    }
}
