using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame.MapObject
{
    public class BlockSlider : ToggleObject
    {
        public GameObject targetObject;
        public Vector2 activePosition;
        public Vector2 inactivePosition;
        public float movingSpeed = 10;
        protected override void ActiveBehavior()
        {
            MoveTo(activePosition);
        }
        protected override void InactiveBehavior()
        {
            MoveTo(inactivePosition);
        }
        private void MoveTo(Vector2 position)
        {
            Vector2 direction = position - (Vector2)targetObject.transform.position;
            if (direction.magnitude < 0.01f)
            {
                targetObject.transform.position = position;
                return;
            }
            targetObject.transform.Translate((Vector3)(direction.normalized * movingSpeed * Time.deltaTime));
        }
    }
}