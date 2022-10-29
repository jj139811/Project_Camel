using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame.Character
{
    public class HeadContainer : Container
    {
        private PlayerCharacter character;
        public float moveSpeed {get; set;} = 10;
        public float jumpSpeed {get; set;} = 10;

        protected override void Awake()
        {
            base.Awake();
            character = gameObject.GetComponent<PlayerCharacter>();
            if (character == null)
            {
                throw new System.Exception("cannot find PlayerCharacter of : " + gameObject.name);
            }
            character.moveSpeed = 0.0f;
            character.jumpSpeed = 0.0f;
        }

        protected override void OnCollisionOccur(Collider2D collider)
        {
            base.OnCollisionOccur(collider);
            if (slotIndex >= 1)
            {
                character.moveSpeed = moveSpeed;
            }
            if (slotIndex >= 2)
            {
                character.jumpSpeed = jumpSpeed;
            }
        }
    }
}
