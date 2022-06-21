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
        private bool jumping;
        private float jumpingDt;
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
            physics.controlVelocity = direction * playerCharacter.moveSpeed;
        }
        public void Jump()
        {
            if (physics.onGround)
            {
                physics.velocity = Vector2.up * playerCharacter.jumpSpeed;
                jumping = true;
                jumpingDt = 0;
            }
        }
        public void JumpPressing ()
        {
            if (jumping)
            {
                jumpingDt += Time.deltaTime;
                if (jumpingDt > playerCharacter.maxJumpDuration)
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
            jumping = false;
        }
    }
}
