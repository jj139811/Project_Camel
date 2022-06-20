using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame.Physics
{
    public class CharacterPhysics : MonoBehaviour
    {
        private BoxCollider2D characterCollider;
        public Vector2 velocity {private get; set;}
        public Vector2 controlVelocity {private get; set;}
        private void Awake()
        {
            characterCollider = gameObject.GetComponent<BoxCollider2D>();
            if (characterCollider == null) {
                throw new System.Exception("Collider not found from : " + gameObject.name);
            }
            velocity = Vector2.zero;
            controlVelocity = Vector2.zero;
        }

        protected Vector2 CheckCollision(Vector2 pos, Vector2 vel) {
            const float collisionTolerance = 0.0f;
            Vector2 displacement = vel * Time.deltaTime;
            RaycastHit2D[] hits = Physics2D.BoxCastAll(pos + displacement.normalized * collisionTolerance, characterCollider.size, 0, displacement.normalized, displacement.magnitude);

            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];

                if (hit.collider == characterCollider) {
                    continue;
                }
                return displacement.normalized * ((hit.distance - collisionTolerance > 0)? (hit.distance - collisionTolerance) : 0 );
            }
            return displacement;
        }

        protected Vector2 GetFinalDisplacement () {
            const float collisionTolerance = 0.0f;

            Vector2 currentPosition = new Vector2 (transform.position.x, transform.position.y);
            Vector2 defaultDisplacement = CheckCollision(currentPosition, velocity);
            
            Vector2 displacement = controlVelocity * Time.deltaTime;
            RaycastHit2D[] hits = Physics2D.BoxCastAll(currentPosition + defaultDisplacement + displacement.normalized * collisionTolerance, characterCollider.size, 0, displacement.normalized, displacement.magnitude);
            
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];

                if (hit.collider == characterCollider) {
                    continue;
                }
                Vector2 collisionDisplacement = displacement.normalized * ((hit.distance - collisionTolerance > 0)? (hit.distance - collisionTolerance) : 0 );
                Vector2 remainingDisplacement = displacement - collisionDisplacement;

                Vector2 colliderNormal = hit.normal.normalized;
                float projectionMagnitude = remainingDisplacement.x * colliderNormal.x + remainingDisplacement.y * colliderNormal.y;
                
                if (projectionMagnitude < 0)
                {
                    Vector2 projection = projectionMagnitude * colliderNormal;
                    return defaultDisplacement + displacement - projection;
                }
            }
            return defaultDisplacement + displacement;
        }

        private void FixedUpdate()
        {
            Vector2 displacement = GetFinalDisplacement();
            transform.Translate(displacement);
        }
    }   
}
