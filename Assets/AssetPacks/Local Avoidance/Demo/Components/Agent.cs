using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace ProjectDawn.LocalAvoidance
{
    public class Agent : MonoBehaviour
    {
        public float3 Destination;
        public float Speed = 1;
        public float TurnSpeed = 10;
        public float Radius = 0.5f;
        public float StopDistance = 0.2f;
        public float3 Velocity;
        public float3 DesiredDirection;

        public float3 Position => transform.position;
        //public float3 DesiredDirection => math.normalizesafe(Destination - (float3)transform.position);
        public bool IsStopped => math.distance(Destination, transform.position) < StopDistance + 0.01f;
    }
}
