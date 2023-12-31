using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {

    [CreateAssetMenu(menuName = "Config/Card/Draw cards")]
    public class DrawCards : CardConfig {

        #region Fields
        [ClassHeader(typeof(DrawCards))]

        [SerializeField] private int lv1NumCard;
        [SerializeField] private int lv2NumCard;
        [SerializeField] private int lv3NumCard;
        #endregion

        #region Publics
        public override void PlayLv1() {
            CardManager.Instance.DrawFromDeck(lv1NumCard);
        }

        public override void PlayLv2() {
            CardManager.Instance.DrawFromDeck(lv2NumCard);
        }

        public override void PlayLv3() {
            CardManager.Instance.DrawFromDeck(lv3NumCard);
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
