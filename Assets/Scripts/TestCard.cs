using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace CSCI526GameJam {
    public class TestCard : MonoBehaviour {

        #region Fields
        [SerializeField] private TMP_Text costText;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private Card card;
        #endregion

        #region Publics
        public Card Card => card;

        public void Init(Card card) {
            this.card = card;

            costText.text = "Cost: " + card.Config.Cost.ToString();
            nameText.text = card.Config.CardName;
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
