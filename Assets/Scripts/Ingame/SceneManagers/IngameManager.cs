using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ingame.Control;
using UnityEngine.SceneManagement;
using Ingame.Character;
using Ingame.Physics;

namespace Ingame.Manager
{
    public class IngameManager : MonoBehaviour
    {
        public float mapMinBoundX = -10;
        public float mapMaxBoundX = 100;
        public float mapMinBoundY = -10;
        public float mapMaxBoundY = 10;
        public float cameraBoundMargin = 5;

        public GameObject mainCamera;

        public GameObject characterHead;
        public GameObject[] characterComponents;

        private void Awake()
        {
            if (mainCamera == null)
            {
                throw new System.Exception("main camera not found");
            }
            if (characterHead == null)
            {
                throw new System.Exception("cannot find character head");
            }
            CameraSmoothFollow camFollow = mainCamera.GetComponent<CameraSmoothFollow>();
            camFollow.minBoundX = mapMinBoundX + cameraBoundMargin;
            camFollow.maxBoundX = mapMaxBoundX - cameraBoundMargin;
            camFollow.minBoundY = mapMinBoundY + cameraBoundMargin;
            camFollow.maxBoundY = mapMaxBoundY - cameraBoundMargin;
        }
        private void Update()
        {
            if (CheckFall(characterHead) || Input.GetKeyDown(KeyCode.R))
            {
                ResetGame();
                return;
            }
            if (characterComponents != null)
            {
                for (int i = 0; i < characterComponents.Length; i++)
                {
                    if (characterComponents[i] != null)
                    {
                        if (CheckFall(characterComponents[i]))
                        {
                            characterComponents[i] = null;
                        }
                    }
                }
            }
            if (CheckClear(characterHead))
            {
                ShowResult();
            }
        }
        private bool CheckFall(GameObject component)
        {
            if (component.transform.position.y < mapMinBoundY)
            {
                Destroy(component);
                return true;
            }
            return false;
        }
        private bool CheckClear(GameObject head)
        {
            if (head.transform.position.x > mapMaxBoundX)
            {
                return true;
            }
            return false;
        }
        private void ResetGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        private void ShowResult()
        {
            PackResult();
            SceneManager.LoadScene("Result");
        }
        private void PackResult()
        {
            GameObject resultObject = new GameObject("result");
            Element headElement = characterHead.GetComponent<Element>();
            GameObject root = characterHead;
            if (headElement.parent != null)
            {
                root = headElement.parent.gameObject;
            }
            root.transform.position = Vector3.zero;
            DisableAndAddComponent(root, resultObject);
            resultObject.transform.localScale = new Vector3(3, 3, 1);
            DontDestroyOnLoad(resultObject);
        }
        private void DisableAndAddComponent (GameObject component, GameObject resultObject)
        {
            CharacterPhysics physics = component.GetComponent<CharacterPhysics>();
            physics.enablePhysics = false;
            PlayerCharacter character = component.GetComponent<PlayerCharacter>();
            character.enableStateUpdate = false;
            character.state = CharacterState.DEFAULT;
            character.direction = CharacterDirection.RIGHT;

            PlayerInput playerInput = component.GetComponent<PlayerInput>();
            playerInput.disableControl = true;

            Container container = component.GetComponent<Container>();
            if (container != null)
            {
                for (int i = 0; i < container.slotIndex; i++)
                {
                    Slot s = container.slots[i];
                    s.SyncWithParent();
                    DisableAndAddComponent(s.gameObject, resultObject);
                }
            }
            component.transform.parent = resultObject.transform;
        }
    }
}