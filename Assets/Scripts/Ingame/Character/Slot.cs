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
        private Vector2 position;
        private int renderingOrder;

        public Slot (GameObject parent, Vector2 position, int renderingOrder)
        {
            this.parent = parent;
            this.position = position;
            this.gameObject = null;
            this.renderingOrder = renderingOrder;
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

            SpriteRenderer[] sprites = target.GetComponent<Element>()?.sprites;
            if (sprites != null)
            {
                for (int i = 0; i < sprites.Length; i++)
                {
                    sprites[i].sortingOrder = renderingOrder;
                }
            }
            SyncWithParent();
        }

        public void SyncWithParent ()
        {
            if (this.gameObject != null)
            {
                if (parentCharacter.direction == CharacterDirection.RIGHT)
                {
                    this.gameObject.transform.position = (Vector2)parent.transform.position + this.position;
                }
                else
                {
                    this.gameObject.transform.position = (Vector2)parent.transform.position + new Vector2(-this.position.x, this.position.y);
                }
                
                if (targetCharacter != null)
                {
                    targetCharacter.state = parentCharacter.state;
                }
            }
        }
        public GameObject PopObject ()
        {
            GameObject ret = this.gameObject;
            this.gameObject = null;
            this.targetCharacter = null;
            return ret;
        }
    }
}
