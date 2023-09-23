using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {

    public abstract class ItemConfig : ScriptableObject {

        #region Fields
        [ClassHeader(typeof(ItemConfig))]

        [MandatoryFields]
        [SerializeField] private string itemName;
        [SerializeField] private ItemRank rank;
        [SerializeField] private int price;
        [SerializeField] private int numAvailable;

        [TextArea(5, 10)]
        [SerializeField] private string description;
        [SerializeField] private Sprite image;
        #endregion

        #region Publics
        public string ItemName => itemName;
        public ItemRank Rank => rank;
        public int Price => price;
        public int NumAvailable => numAvailable;

        public string Description => description;
        public Sprite Image => image;

        public abstract void OnAdd();
        public abstract void OnRemove();
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
