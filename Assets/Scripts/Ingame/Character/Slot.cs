using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ingame.Physics;

namespace Ingame.Character
{
    public class Slot
    {
        private GameObject gameObject;
        private GameObject parent;
        private PlayerCharacter targetCharacter;
        private PlayerCharacter parentCharacter;
        public Vector2 position;

        public Slot (GameObject parent, Vector2 position)
        {
            this.parent = parent;
            this.position = position;
            this.gameObject = null;
            this.parentCharacter = parent.GetComponent<PlayerCharacter>();
            if (parentCharacter == null) {
                throw new System.Exception("Cannot find PlayerCharacter of : " + parent.name);
            }
        }
        public bool CheckObject (GameObject target)
        {
            return this.gameObject == target;
        }
        public void AddObject (GameObject target)
        {
            this.gameObject = target;
            CharacterPhysics physics = target.GetComponent<CharacterPhysics>();
            if (physics != null)
            {
                physics.enablePhysics = false;
            }
            this.targetCharacter = target.GetComponent<PlayerCharacter>();
            if (targetCharacter == null) {
                throw new System.Exception("Cannot find PlayerCharacter of : " + target.name);
            }
            targetCharacter.enableStateUpdate = false;
        }

        public void SyncWithParent ()
        {
            if (this.gameObject != null)
            {
                this.gameObject.transform.position = (Vector2)parent.transform.position + this.position;
                if (targetCharacter != null)
                {
                    targetCharacter.state = parentCharacter.state;
                }
            }
        }
    }
}
