using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CSCI526GameJam
{
    public class UIShop : MonoBehaviour
    {
        public GameObject ShopPanel;
        public Button shopButton;
        public Button closeButton;
        public TextMeshProUGUI goldsInfo;
        // Start is called before the first frame update
        void Start()
        {
            shopButton.onClick.AddListener(OpenShop);
            closeButton.onClick.AddListener(CloseShop);
            ShopPanel.SetActive(false);
        }
        void Update()
        {
            UpdateGoldsInfo();
        }
        
        private void UpdateGoldsInfo()
        {
            goldsInfo.text = "Golds: " + Player.Instance.Gold.ToString();
        }
        private void OpenShop()
        {
            ShopPanel.SetActive(true);
        }

        private void CloseShop()
        {
            ShopPanel.SetActive(false);
        }
        
        public void DisableButtonOnClick(Button button)
        {
            button.interactable = false;
        }
    }
}
