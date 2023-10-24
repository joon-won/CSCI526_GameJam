using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam.Buffs {

    [Serializable]
    public abstract class Buff {

        #region Fields
        [ComputedFields]
        [SerializeField] protected Buffable target;
        [SerializeField] protected BuffConfig config;
        
        [SerializeField] protected float duration;
        [SerializeField] protected float elapsed;
        
        [SerializeField] protected int numStacks;
        #endregion

        #region Publics
        public BuffConfig Config => config;
        
        public bool IsExpired {
            get {
                if (config.IsPermanent) return false;
                return elapsed >= duration;
            }
        }

        public void Apply() {
            if (numStacks == 0 || config.IsDurationStackable) {
                duration += config.Duration;
            }
            
            if (numStacks == 0 || config.IsEffectStackable) {
                ApplyEffect();
                numStacks++;
            }
        }

        public void End() {
            for (int i = 0; i < numStacks; i++) {
                EndEffect();
            }
        }

        public void Update() {
            if (config.IsPermanent) return;
            if (IsExpired) return;

            elapsed += Time.deltaTime;
            UpdateEffect();
        }
        #endregion

        #region Internals
        protected abstract void ApplyEffect();
        protected abstract void EndEffect();
        protected abstract void UpdateEffect();

        protected Buff(BuffConfig config, Buffable target) {
            this.config = config;
            this.target = target;
            duration = 0f;
            elapsed = 0f;
            numStacks = 0;
        }
        #endregion

        #region Unity Methods
        #endregion
    }
}
