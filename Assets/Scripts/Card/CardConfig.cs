using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace CSCI526GameJam {

    public abstract class CardConfig : ScriptableObject {

        #region Fields
        [ClassHeader(typeof(CardConfig))]

        [SerializeField] private string cardName;
        [SerializeField] private ItemRank rank;
        [SerializeField] private int cost;

        [TextArea(5, 10)]
        [SerializeField] private string description;
        [SerializeField] private Sprite image;
        #endregion

        #region Publics
        public string CardName => cardName;
        public ItemRank Rank => rank;
        public int Cost => cost;

        public string Description => description;
        public Sprite Image => image;

        public abstract void PlayLv1();
        public abstract void PlayLv2();
        public abstract void PlayLv3();
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
