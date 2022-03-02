using UnityEngine;
using Unity.Mathematics;

namespace ProjectDawn.LocalAvoidance.Demo
{
    /// <summary>
    /// Spawns agent with count and radius.
    /// </summary>
    public class AgentSpawnerSystem : System
    {
        public FollowAgent Prefab;
        public Agent FollowTarget;
        public int Count;
        public float Radius;

        void Start()
        {
            if (Prefab == null)
                return;

            var rnd = new Unity.Mathematics.Random(1);
            for (int i = 0; i < Count; ++i)
            {
                var direction = rnd.NextFloat2Direction();
                var position = new float3(direction.x, 0, direction.y) * Radius * rnd.NextFloat();

                var instance = GameObject.Instantiate(Prefab.gameObject);
                instance.transform.position = position;

                var followAgent = instance.GetComponent<FollowAgent>();
                followAgent.TargetAgent = FollowTarget;
            }
        }
    }
}
