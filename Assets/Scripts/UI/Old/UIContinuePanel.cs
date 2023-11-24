using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CSCI526GameJam
{
    public class UIContinuePanel : MonoBehaviour
    {
        public GameObject ContinuePanel;

        public Button continueButton;
        // Start is called before the first frame update
        void Start()
        {
            continueButton.onClick.AddListener(GameContinue);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void GameContinue()
        {
            ContinuePanel.SetActive(false);
        }
    }
}
