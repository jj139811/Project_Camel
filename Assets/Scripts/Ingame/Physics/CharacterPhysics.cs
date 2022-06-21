using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame.Physics
{
    public class CharacterPhysics : MonoBehaviour
    {
        const float collisionTolerance = 0.0f;
        private BoxCollider2D characterCollider;
        public Vector2 velocity {private get; set;}
        public Vector2 controlVelocity {private get; set;}
        public bool onGround {get; private set;}
        public Vector2 gravity {get; private set;} = Vector2.down * 20;

        private Vector2 colliderSize
        {
            get
            {
                return new Vector2 (
                    characterCollider.size.x * transform.localScale.x,
                    characterCollider.size.y * transform.localScale.y
                );
            }
        }
        private void Awake()
        {
            characterCollider = gameObject.GetComponent<BoxCollider2D>();
            if (characterCollider == null) {
                throw new System.Exception("Collider not found from : " + gameObject.name);
            }
            velocity = Vector2.zero;
            controlVelocity = Vector2.zero;
        }

        private bool OnGround () {
            const float groundDetectionTolerance = 0.01f;
            if ((velocity + controlVelocity).y > groundDetectionTolerance) {
                return false;
            }

            Vector2 gravityDirection = (gravity.magnitude == 0)? Vector2.down : gravity.normalized;
            RaycastHit2D[] hits = Physics2D.BoxCastAll(new Vector2(transform.position.x, transform.position.y) + gravityDirection * groundDetectionTolerance, colliderSize - new Vector2(groundDetectionTolerance, 0), 0, gravityDirection, 0);

            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];
                if (hit.collider == characterCollider)
                {
                    continue;
                }
                return true;
            }
            return false;
        }

        private Vector2 CheckCollision(Vector2 pos, Vector2 vel) {
            Vector2 displacement = vel * Time.deltaTime;
            RaycastHit2D[] hits = Physics2D.BoxCastAll(pos, colliderSize, 0, displacement.normalized, displacement.magnitude);

            float minDistance = displacement.magnitude;
            
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];

                if (hit.collider == characterCollider) {
                    continue;
                }
                if (hit.normal.x * vel.x + hit.normal.y * vel.y >= 0)
                {
                    continue;
                }
                float newDistance = (hit.distance - collisionTolerance > 0)? (hit.distance - collisionTolerance) : 0;

                if (newDistance < minDistance) {
                    minDistance = newDistance;
                }
            }
            return displacement.normalized * minDistance;
        }

        private Vector2 GetFinalDisplacement () {
            Vector2 currentPosition = new Vector2 (transform.position.x, transform.position.y);
            Vector2 defaultDisplacement = CheckCollision(currentPosition, velocity);
            
            Vector2 displacement = controlVelocity * Time.deltaTime;
            RaycastHit2D[] hits = Physics2D.BoxCastAll(currentPosition + defaultDisplacement, colliderSize, 0, displacement.normalized, displacement.magnitude);
            
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];

                if (hit.collider == characterCollider) {
                    continue;
                }
                Vector2 collisionDisplacement = displacement.normalized * ((hit.distance > 0)? hit.distance : 0 );
                Vector2 remainingDisplacement = displacement - collisionDisplacement;

                Vector2 colliderNormal = hit.normal.normalized;
                float projectionMagnitude = remainingDisplacement.x * colliderNormal.x + remainingDisplacement.y * colliderNormal.y;
                
                if (projectionMagnitude < 0)
                {
                    Vector2 projection = projectionMagnitude * colliderNormal;
                    displacement = displacement - projection;
                }
            }
            return defaultDisplacement + displacement;
        }

        private void FixedUpdate()
        {
            if (controlVelocity.y <= 0) {
                velocity += gravity * Time.deltaTime;
            }
            else
            {
                velocity = Vector2.zero;
            }
            Vector2 displacement = GetFinalDisplacement();
            transform.Translate(displacement);

            onGround = OnGround();
            if (onGround) 
            {
                velocity = Vector2.zero;
            }
        }
    }   
}
