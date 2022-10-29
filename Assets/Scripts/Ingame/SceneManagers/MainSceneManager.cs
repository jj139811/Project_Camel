using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ingame.Manager
{
    public class MainSceneManager : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Next();
            }
        }
        public void Next()
        {
            SceneManager.LoadScene("StageSelect");
        }
    }
}