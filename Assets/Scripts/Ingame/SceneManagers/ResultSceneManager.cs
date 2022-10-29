using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ingame.Manager
{
    public class ResultSceneManager : MonoBehaviour
    {
        private GameObject resultObject;
        private void Start()
        {
            resultObject = GameObject.Find("result");
            if (resultObject == null)
            {
                throw new System.Exception("cannot find result object!");
            }
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Next();
            }
        }
        public void Next()
        {
            Destroy(resultObject);
            SceneManager.LoadScene("Main");
        }
    }
}