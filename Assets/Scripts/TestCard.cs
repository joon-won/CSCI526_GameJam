using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace CSCI526GameJam {
    public class TestCard : MonoBehaviour {

        #region Fields
        [MandatoryFields]
        [SerializeField] private TMP_Text costText;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Image image;

        [ComputedFields]
        [SerializeField] private Card card;
        #endregion

        #region Publics
        public Card Card => card;

        public void Init(Card card) {
            this.card = card;
            Refresh();
        }

        public void Refresh() {
            costText.text = card.Cost.ToString();
            nameText.text = card.Name;
            descriptionText.text = card.GetDescription();
            image.sprite = card.Image;
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
