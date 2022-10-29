using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame.MapObject
{
    public abstract class ToggleObject : MonoBehaviour
    {
        BoxCollider2D boxCollider2D;
        protected bool isActive;
        private bool prevCollision;
        protected void Awake()
        {
            isActive = false;
            prevCollision = false;
            boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
            if (boxCollider2D == null)
            {
                throw new System.Exception("cannot find box collider of: " + gameObject.name);
            }
        }
        protected void FixedUpdate()
        {
            CheckInteraction();
            if (isActive)
            {
                ActiveBehavior();
            }
            else
            {
                InactiveBehavior();
            }
        }
        private bool CheckInteraction ()
        {
            RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, boxCollider2D.size, 0, Vector2.up);
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];
                if (hit.transform.gameObject == gameObject || hit.distance > 0.01f)
                {
                    continue;
                }
                if (hit.transform.gameObject.tag == "Player")
                {
                    if (!prevCollision)
                    {
                        isActive = !isActive;
                        prevCollision = true;
                        return true;
                    }
                    return false;
                }
            }
            prevCollision = false;
            return false;
        }
        protected abstract void ActiveBehavior();
        protected abstract void InactiveBehavior();
    }
}
