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
        public TMP_Text enemyInfo;
        public Button StartCombat;

        [SerializeField] private TMP_Text levelText;

        // Start is called before the first frame update
        void Start()
        {
            StartCombat.onClick.AddListener(CombatStart);
            CombatPanel.SetActive(false);

            levelText.gameObject.SetActive(false);
            GameManager.Instance.OnBuyingStarted += CombatEnd;
        }
        
        void Update()
        {
            UpdateGoldsInfo();
            UpdateBaseInfo();
            UpdateEnemyInfo();
        }
        
        private void UpdateGoldsInfo()
        {
            goldsInfo.text = "Golds: " + Player.Instance.Gold.ToString();
        }

        private void UpdateBaseInfo() 
        {
            baseInfo.text = "BaseHP: " + TowerManager.Instance.PlayerBase.Health.ToString();
        }

        private void UpdateEnemyInfo() 
        {
            enemyInfo.text = "Enemies: " + EnemyManager.Instance.NumEnemies.ToString();
        }

        private void CombatStart()
        {
            CombatPanel.SetActive(true);
            ControlPanel.SetActive(false);
            StartCoroutine(LevelTextRoutine());
        }

        private IEnumerator LevelTextRoutine() {
            levelText.gameObject.SetActive(true);
            levelText.text = $"Level {GameManager.Instance.Level}";
            yield return new WaitForSeconds(3f);

            levelText.gameObject.SetActive(false);
        }

        private void CombatEnd() {
            CombatPanel.SetActive(false);
            ControlPanel.SetActive(true);
        }
    }
}
