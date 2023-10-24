using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam.Buffs {

    public abstract class BuffConfig : ScriptableObject {

        #region Fields
        [ClassHeader(typeof(BuffConfig))]

        [SerializeField] protected bool isPermanent;
        [SerializeField] protected bool isEffectStackable;
        [SerializeField] protected bool isDurationStackable;
        
        [SerializeField] protected float duration;
        #endregion

        #region Publics
        public bool IsPermanent => isPermanent;
        public float Duration => duration;
        public bool IsEffectStackable => isEffectStackable;
        public bool IsDurationStackable => isDurationStackable;

        public abstract Buff ToBuff(Buffable target);
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
