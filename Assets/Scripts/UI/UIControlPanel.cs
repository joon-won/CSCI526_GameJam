using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CSCI526GameJam
{
    public class UIControlPanel : MonoBehaviour
    {
        public TextMeshProUGUI goldsInfo;
        [SerializeField] private List<Button> towerButtons;

        // Update is called once per frame
        void Update()
        {
            UpdateGoldsInfo();
        }

        private void UpdateGoldsInfo()
        {
            goldsInfo.text = "Golds: " + Player.Instance.Gold.ToString();
        }

        private void RefreshTowerButtons() {
            for (int i = 0; i < towerButtons.Count; i++) {
                var button = towerButtons[i];
                var config = Player.Instance.GetTowerConfig(i);
                button.GetComponentInChildren<TMP_Text>().text = config.name;
            }
        }

        private void Start() {

            // Set up button onClick
            for (int i = 0; i < towerButtons.Count; i++) {
                var button = towerButtons[i];
                var index = i;
                button.onClick.AddListener(() => Player.Instance.PickTower(index));
            }
            Player.Instance.OnTowerPoolChanged += RefreshTowerButtons;
            RefreshTowerButtons();
        }
    }
}
