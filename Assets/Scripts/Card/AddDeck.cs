using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CSCI526GameJam {

    [CreateAssetMenu(menuName = "Config/Card/Add Deck")]
    public class AddDeck : CardConfig {

        #region Fields
        [SerializeField] private int lv1;
        [SerializeField] private int lv2;
        [SerializeField] private int lv3;
        #endregion

        #region Publics
        public override void PlayLv1() {
            CardManager.Instance.AddDeck(lv1);
        }

        public override void PlayLv2() {
            CardManager.Instance.AddDeck(lv2);
        }

        public override void PlayLv3() {
            CardManager.Instance.AddDeck(lv3);
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
