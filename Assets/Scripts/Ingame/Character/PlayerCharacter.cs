using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ingame.Physics;
using Ingame.Control;

namespace Ingame.Character
{
    public enum CharacterState {
        DEFAULT,
        RUN,
        JUMP,
        FALL,
        ATTACK,
        GUARD
    }
    public enum CharacterDirection {
        LEFT,
        RIGHT
    }
    public class PlayerCharacter: MonoBehaviour
    {
        public CharacterState state {get; set;}
        public CharacterDirection direction {get; private set;}
        private CharacterPhysics physics;
        private PlayerInput control;
        public float moveSpeed {get; private set;} = 10;
        public float jumpSpeed {get; private set;} = 10;
        public float maxJumpDuration {get; private set;} = 0.5f;
        public bool enableStateUpdate = true;
        private void Awake() {
            state = CharacterState.DEFAULT;
            direction = CharacterDirection.RIGHT;
            physics = gameObject.GetComponent<CharacterPhysics>();
            if (physics == null) {
                throw new System.Exception("Cannot find CharacterPhysics of : " + gameObject.name);
            }
            control = gameObject.GetComponent<PlayerInput>();
            if (control == null) {
                throw new System.Exception("Cannot find PlayerInput of : " + gameObject.name);
            }
        }
        private void Update () {
            UpdateState();
        }
        public void SetDirection (CharacterDirection direction) {
            this.direction = direction;
        }
        public void UpdateState () {
            if (!enableStateUpdate)
            {
                return;
            }
            if (physics.onGround) {
                if (control.moving) {
                    state = CharacterState.RUN;
                }
                else {
                    state = CharacterState.DEFAULT;
                }
            }
            else {
                if (control.jumping) {
                    state = CharacterState.JUMP;
                }
                else {
                    state = CharacterState.FALL;
                }
            }
        }
    }
}
