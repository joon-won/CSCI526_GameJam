using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System.Linq;
using Sirenix.OdinInspector;

namespace CSCI526GameJam {

    public abstract class CardConfig : ScriptableObject {

        #region Fields
        [ClassHeader(typeof(CardConfig))]

        [SerializeField] private string cardName;
        [SerializeField] private int cost;

        [SerializeField, TextArea(5, 10)]
        private string[] descriptions;
        [SerializeField, PreviewField(200f)]
        private Sprite image;
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
