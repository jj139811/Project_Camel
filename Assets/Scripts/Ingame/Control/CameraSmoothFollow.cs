using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame.Control
{
    public class CameraSmoothFollow : MonoBehaviour
    {
        public Transform target;
        public float minSpeed = 5.0f;
        public float maxSpeed = 40.0f;
        public float v = 2.0f;

        private void FixedUpdate() {
            // Calculate velocity
            Vector2 diff = target.position - transform.position;
            Vector2 velocity;
            if (diff.magnitude * v == 0)
            {
                velocity = Vector2.zero;
            }
            else if (diff.magnitude * v < minSpeed)
            {
                velocity = minSpeed * diff.normalized;
            }
            else if (diff.magnitude * v > maxSpeed)
            {
                velocity = maxSpeed * diff.normalized;
            }
            else
            {
                velocity = diff * v;
            }

            // Update position
            Vector2 displacement = velocity * Time.deltaTime;
            if (displacement.magnitude > diff.magnitude)
            {
                displacement = diff;
            }

            transform.Translate(displacement);
        }
    }
}