using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CSCI526GameJam {
    public class UIModeText : MonoBehaviour {

        [SerializeField] private TMP_Text modeText;

        private void Update() {
            var mode = Player.Instance.CurrentMode;
            
            modeText.enabled = true;
            switch (mode) {
                case Player.Mode.Build:
                    modeText.text = "Placing";
                    break;
                
                case Player.Mode.Demolish:
                    modeText.text = "Removing";
                    break;
                
                default:
                    modeText.enabled = false;
                    break;
            }
        }
    }
}
