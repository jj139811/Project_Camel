using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Art
{
    public class BackgroundFollowCamera : MonoBehaviour
    {
        public float c = 1.0f;
        public GameObject mainCamera;
        private void Awake()
        {
            if (mainCamera == null)
            {
                throw new System.Exception("Main camera not set");
            }
        }

        private void Update ()
        {
            Vector3 newpos = mainCamera.transform.position * c;
            transform.position = new Vector3(newpos.x, newpos.y, 0);
        }
    }
}
