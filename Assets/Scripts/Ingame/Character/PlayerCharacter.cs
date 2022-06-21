using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame.Character
{
    public class PlayerCharacter: MonoBehaviour
    {
        public float moveSpeed {get; private set;} = 10;
        public float jumpSpeed {get; private set;} = 10;
        public float maxJumpDuration {get; private set;} = 0.5f;
        private void Awake() {
            
        }
    }
}
