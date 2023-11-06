using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {

    [CreateAssetMenu(menuName = "Config/Tutorial")]
    public class TutorialConfig : ScriptableObject {

        #region Fields
        [SerializeField] private int gold;
        [SerializeField] private CardConfig[] deckPreset;
        [SerializeField] private CardConfig[] onHandCardsPreset;
        #endregion

        #region Publics
        public int Gold => gold;
        public CardConfig[] DeckPreset => deckPreset;
        public CardConfig[] OnHandCardsPreset => onHandCardsPreset;
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
