using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ingame.Animation;

namespace Ingame.Character
{
    public class Container : MonoBehaviour
    {
        protected Slot[] slots;
        public int numSlots = 4;
        public Vector2[] slotPositions;
        protected int slotIndex;
        private BoxCollider2D boxCollider2D;
        protected virtual void Awake()
        {
            boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
            if (boxCollider2D == null)
            {
                throw new System.Exception("cannot find box collider!!");
            }
            if (slotPositions.Length != numSlots)
            {
                throw new System.Exception("slot number mismatch");
            }
            slots = new Slot[numSlots];
            for (int i = 0; i < numSlots; i++)
            {
                slots[i] = new Slot(gameObject, slotPositions[i]);
            }
            slotIndex = 0;
        }
        private void Update ()
        {
            RaycastHit2D[] hits = Physics2D.BoxCastAll((Vector2)transform.position
            + boxCollider2D.offset, boxCollider2D.size, 0.0f, Vector2.up);
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];
                if (hit.collider == boxCollider2D)
                {
                    continue;
                }
                if (hit.distance > 0.01f)
                {
                    continue;
                }
                OnCollisionOccur(hit.collider);
            }
            for (int i = 0; i < numSlots; i++)
            {
                slots[i].SyncWithParent();
            }
        }
        protected virtual void OnCollisionOccur (Collider2D collider)
        {
            GameObject target = collider.gameObject;
            if (target.tag == "Player")
            {
                for (int i = 0; i < slotIndex; i++)
                {
                    if (slots[i].CheckObject(target))
                    {
                        return;
                    }
                }
                if (slotIndex < slots.Length)
                {
                    CharacterAnimationManager animationManager = target.GetComponent<CharacterAnimationManager>();
                    if (animationManager != null)
                    {
                        animationManager.flipRun = (slotIndex % 2 == 0);
                    }
                    slots[slotIndex].AddObject(target);
                    slotIndex += 1;
                }
            }
        }
    }
}
