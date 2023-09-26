using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CSCI526GameJam
{
    public class UIControlPanel : MonoBehaviour
    {
        public TextMeshProUGUI goldsInfo;

        // Update is called once per frame
        void Update()
        {
            UpdateGoldsInfo();
        }

        private void UpdateGoldsInfo()
        {
            goldsInfo.text = "Golds: " + Player.Instance.Gold.ToString();
        }
    }
}
