using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CSCI526GameJam
{
    public class UICombat : MonoBehaviour
    {
        public GameObject CombatPanel;
        public GameObject ControlPanel;

        public Button StartCombat;
        // Start is called before the first frame update
        void Start()
        {
            StartCombat.onClick.AddListener(CombatStart);
            CombatPanel.SetActive(false);
        }

        private void CombatStart()
        {
            CombatPanel.SetActive(true);
            ControlPanel.SetActive(false);
        }
    }
}
