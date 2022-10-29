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
        public int[] slotRenderingOrders;
        public int containerOrder = 0;
        public int slotIndex;
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
                throw new System.Exception("slot position number mismatch");
            }
            if (slotRenderingOrders.Length != numSlots)
            {
                throw new System.Exception("slot order number mismatch");
            }
            slots = new Slot[numSlots];
            for (int i = 0; i < numSlots; i++)
            {
                slots[i] = new Slot(gameObject, slotPositions[i], slotRenderingOrders[i]);
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
        public GameObject PopChild ()
        {
            if (slotIndex <= 0)
            {
                return null;
            }
            slotIndex -= 1;
            return slots[slotIndex].PopObject();
        }
        protected virtual void OnCollisionOccur (Collider2D collider)
        {
            GameObject target = collider.gameObject;
            Element targetElement = target.GetComponent<Element>();
            if (targetElement != null)
            {
                if (targetElement.parent != null)
                {
                    return;
                }
                if (slotIndex < slots.Length)
                {
                    
                    AddToSlot(target);
                }
            }
        }
        private void AddToSlot(GameObject target)
        {
            CharacterAnimationManager animationManager = target.GetComponent<CharacterAnimationManager>();
            Element targetElement = target.GetComponent<Element>();
            Container targetContainer = target.GetComponent<Container>();
            if (targetContainer != null)
            {
                int targetOrder = targetContainer.containerOrder;
                if (targetOrder == containerOrder)
                {
                    throw new System.Exception("Container orders cannot be equal");
                }
                if (targetOrder < containerOrder)
                {
                    return;
                }
            }

            if (animationManager != null)
            {
                animationManager.flipRun = (slotIndex % 2 == 0);
            }
            slots[slotIndex].AddObject(target);
            targetElement.parent = this;
            slotIndex += 1;

            ResizeBoundary(target);

            if (targetContainer != null)
            {
                GameObject targetChild = targetContainer.PopChild();
                while (targetChild != null)
                {
                    AddToSlot(targetChild);
                    if (slotIndex >= slots.Length)
                    {
                        break;
                    }
                    targetChild = targetContainer.PopChild();
                }
            }
        }
        private void ResizeBoundary (GameObject target)
        {
            BoxCollider2D targetCollider = target.GetComponent<BoxCollider2D>();
            if (targetCollider == null)
            {
                return;
            }
            Vector2 targetPos = targetCollider.offset + (Vector2)target.transform.position
            - (Vector2)transform.position;
            float targetWidth = targetCollider.size.x;
            float targetHeight = targetCollider.size.y;

            Vector2 originalPos = boxCollider2D.offset;
            float originalWidth = boxCollider2D.size.x;
            float originalHeight = boxCollider2D.size.y;

            float minBoundX = min(targetPos.x - targetWidth / 2, originalPos.x - originalWidth / 2);
            float maxBoundX = max(targetPos.x + targetWidth / 2, originalPos.x + originalWidth / 2);

            float minBoundY = min(targetPos.y - targetHeight / 2, originalPos.y - originalHeight / 2);
            float maxBoundY = max(targetPos.y + targetHeight / 2, originalPos.y + originalHeight / 2);

            float newWidth = maxBoundX - minBoundX;
            float newHeight = maxBoundY - minBoundY;

            Vector2 newPos = new Vector2((maxBoundX + minBoundX) / 2, (maxBoundY + minBoundY) / 2);

            boxCollider2D.offset = newPos;
            boxCollider2D.size = new Vector2(newWidth, newHeight);

            transform.Translate(Vector3.down * (minBoundY - (originalPos.y - originalHeight / 2)));
        }
        private float min(float a, float b)
        {
            if (a > b)
            {
                return b;
            }
            return a;
        }
        private float max(float a, float b)
        {
            if (a < b)
            {
                return b;
            }
            return a;
        }
    }
}
