using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ingame.Physics;
using Ingame.Character;

namespace Ingame.Control {
    public class PlayerInput: MonoBehaviour
    {
        private PlayerCharacter playerCharacter;
        private CharacterPhysics physics;
        public bool moving {get; private set;}
        public bool jumping {get; private set;}
        private float jumpingDt;
        public bool disableControl = false;
        private void Awake () {
            playerCharacter = gameObject.GetComponent<PlayerCharacter>();
            if (playerCharacter == null) {
                throw new System.Exception("Cannot find PlayerCharacter of : " + gameObject.name);
            }
            physics = gameObject.GetComponent<CharacterPhysics>();
            if (physics == null) {
                throw new System.Exception("Cannot fin CharacterPhysics of : " + gameObject.name);
            }

            jumping = false;
        }
        public void Move(Vector2 direction)
        {
            if (disableControl)
            {
                return;
            }
            if (direction.magnitude < 0.001f)
            {
                moving = false;
            }
            else
            {
                moving = true;
                
                playerCharacter.SetDirection(direction.x > 0? CharacterDirection.RIGHT: CharacterDirection.LEFT);
            }
            if (physics.onGround)
            {
                Vector2 normal = physics.stepHitInfo.normal;
                float projectionMagnitude = direction.x * normal.x + direction.y * normal.y;
                Vector2 remaining = direction - projectionMagnitude * normal;
                physics.controlVelocity = remaining * playerCharacter.moveSpeed;
                return;
            }
            physics.controlVelocity = direction * playerCharacter.moveSpeed;
        }
        public void Stop()
        {
            if (disableControl)
            {
                return;
            }
            moving = false;
        }
        public void Jump()
        {
            if (disableControl)
            {
                return;
            }
            if (physics.onGround)
            {
                physics.velocity = Vector2.up * playerCharacter.jumpSpeed;
                jumping = true;
                jumpingDt = 0;
            }
        }
        public void JumpPressing ()
        {
            if (disableControl)
            {
                return;
            }
            if (jumping)
            {
                jumpingDt += Time.deltaTime;
                if (jumpingDt > playerCharacter.maxJumpDuration || physics.onHead)
                {

                    jumping = false;
                }
                else
                {
                    physics.velocity = Vector2.up * playerCharacter.jumpSpeed;
                }
            }
        }
        public void JumpUp () {
            if (disableControl)
            {
                return;
            }
            jumping = false;
        }
    }
}
