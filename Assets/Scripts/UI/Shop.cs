using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CSCI526GameJam
{
    public class Shop : MonoBehaviour
    {
        public GameObject ShopPanel;
        public Button shopButton;
        public Button closeButton;
        // Start is called before the first frame update
        void Start()
        {
            shopButton.onClick.AddListener(OpenShop);
            closeButton.onClick.AddListener(CloseShop);
            ShopPanel.SetActive(false);
        }

        private void OpenShop()
        {
            ShopPanel.SetActive(true);
        }

        private void CloseShop()
        {
            ShopPanel.SetActive(false);
        }
    }
}
