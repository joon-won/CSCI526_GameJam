using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CSCI526GameJam
{
    public class UICombat : MonoBehaviour
    {
        public GameObject CombatPanel;
        public GameObject ControlPanel;
        public TextMeshProUGUI goldsInfo;
        public TextMeshProUGUI baseInfo;
        public Button StartCombat;
        // Start is called before the first frame update
        void Start()
        {
            StartCombat.onClick.AddListener(CombatStart);
            CombatPanel.SetActive(false);
        }
        
        void Update()
        {
            UpdateGoldsInfo();
            // UpdateBaseInfo();
        }
        
        private void UpdateGoldsInfo()
        {
            goldsInfo.text = "Golds: " + Player.Instance.Gold.ToString();
        }

        // private void UpdateBaseInfo()
        // {
        //     baseInfo.text = "BaseHP: " + TowerManager.Instance.PlayerBase.ToString();
        // }
        
        private void CombatStart()
        {
            CombatPanel.SetActive(true);
            ControlPanel.SetActive(false);
        }
    }
}
