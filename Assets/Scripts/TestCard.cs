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
        #endregion

        #region Publics
        public void Init(Card card) {
            costText.text = card.Cost.ToString();
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
