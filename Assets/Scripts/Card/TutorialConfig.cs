using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {

    [Serializable]
    public struct TutorialInfo {
        public int Gold;
        public CardConfig[] Deck;
        public CardConfig[] OnHandCards;
        public LevelInfo LevelInfo;
    }

    [CreateAssetMenu(menuName = "Config/Tutorial")]
    public class TutorialConfig : ScriptableObject {

        #region Fields
        [SerializeField] private TutorialInfo[] tutorialInfos;
        #endregion

        #region Publics
        public TutorialInfo[] TutorialInfos => tutorialInfos;
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
