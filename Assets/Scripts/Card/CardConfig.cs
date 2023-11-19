using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System.Linq;

namespace CSCI526GameJam {

    public abstract class CardConfig : ScriptableObject {

        #region Fields
        [ClassHeader(typeof(CardConfig))]

        [SerializeField] private string cardName;
        [SerializeField] private int cost;

        [TextArea(5, 10)]
        [SerializeField] private string[] descriptions;
        [SerializeField] private Sprite image;
        #endregion

        #region Publics
        public string CardName => cardName;
        public int Cost => cost;

        public string[] Descriptions => descriptions;
        public Sprite Image => image;

        public abstract void PlayLv1();
        public abstract void PlayLv2();
        public abstract void PlayLv3();
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        private void OnValidate() {
            if (descriptions == null || descriptions.Length != 3) {
                Array.Resize(ref descriptions, 3);
            }
        }
        #endregion
    }
}
