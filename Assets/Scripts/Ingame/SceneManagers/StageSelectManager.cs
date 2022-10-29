using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Ingame.Manager
{
    public class StageSelectManager : MonoBehaviour
    {
        public GameObject[] stageNames;
        public GameObject stageSelector;
        private int numStages;
        private int selectedStage;
        private void Awake()
        {
            if (stageNames == null)
            {
                throw new System.Exception("Stage names should be initiallized");
            }
            numStages = stageNames.Length;
            selectedStage = 0;
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (selectedStage >= 1)
                {
                    selectedStage -= 1;
                    UpdateStageSelectorPosition();
                }
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (selectedStage < numStages - 1)
                {
                    selectedStage += 1;
                    UpdateStageSelectorPosition();
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                LoadStage();
            }
        }
        private void UpdateStageSelectorPosition()
        {
            if (stageSelector == null)
            {
                return;
            }
            stageSelector.GetComponent<RectTransform>().position = stageNames[selectedStage].GetComponent<RectTransform>().position;
        }
        private void LoadStage()
        {
            switch (selectedStage)
            {
                case 0:
                    SceneManager.LoadScene("Stage_01");
                    break;
                case 1:
                    SceneManager.LoadScene("Stage_02");
                    break;
                case 2:
                    SceneManager.LoadScene("Stage_03");
                    break;
                case 3:
                    SceneManager.LoadScene("Stage_04");
                    break;
                default:
                    break;
            }
        }
    }
}
