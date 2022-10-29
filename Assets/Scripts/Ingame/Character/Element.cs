using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame.Character
{
    public class Element : MonoBehaviour
    {
        public Container parent;
        public SpriteRenderer[] sprites;

        private void Awake()
        {
            parent = null;
        }
    }
}