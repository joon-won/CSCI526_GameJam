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
        #endregion

        #region Publics
        public void Init(Card card) {
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
