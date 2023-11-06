using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {

    [Serializable]
    public class Card {

        #region Fields
        [SerializeField] private CardConfig config;
        #endregion

        #region Publics
        public CardConfig Config => config;

        public Card(CardConfig config) {
            this.config = config;
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
