using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CSCI526GameJam {
    public class UITowerInfoPopup : MonoBehaviour {
        
        #region Fields
        [MandatoryFields]
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;

        private RectTransform rectTransform;
        #endregion

        #region Publics
        public void Init() {
            rectTransform = transform as RectTransform;
            Hide();
        }
        
        public void Show(TowerConfig config, RectTransform anchor) {
            gameObject.SetActive(true);
            
            rectTransform.pivot = anchor.pivot;
            rectTransform.position = anchor.position;
            
            nameText.text = config.name;
            descriptionText.text = config.Description;
        }

        public void Hide() {
            gameObject.SetActive(false);
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
