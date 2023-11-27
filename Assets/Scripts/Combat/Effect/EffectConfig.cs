using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CSCI526GameJam {

    public abstract class EffectConfig : ScriptableObject {

        #region Fields
        [ClassHeader(typeof(EffectConfig))]

        [SerializeField] private Sprite image;

        [SerializeField] protected bool isPermanent;
        [SerializeField, HideIf("isPermanent")] 
        protected int duration;
        [SerializeField, HideIf("isPermanent")]
        protected bool isDurationStackable;

        [SerializeField] protected bool isEffectStackable;

        #endregion

        #region Publics
        public Sprite Image => image;

        public bool IsPermanent => isPermanent;
        public int Duration => isPermanent ? -1 : duration;
        public bool IsDurationStackable => isPermanent ? false : isDurationStackable;

        public bool IsEffectStackable => isEffectStackable;


        public abstract Effect ToEffect();
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
