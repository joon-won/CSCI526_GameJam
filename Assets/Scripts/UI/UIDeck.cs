using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

namespace CSCI526GameJam {
    public class UIDeck : MonoBehaviour {

        #region Fields
        [MandatoryFields]
        [SerializeField] private TMP_Text deckNumText;
        #endregion

        #region Publics
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        private void Update() {
            deckNumText.text = CardManager.Instance.NumInDeck.ToString();
        }
        #endregion
    }
}
