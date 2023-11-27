using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {

    [Serializable]
    public abstract class Effect {

        #region Fields
        [ComputedFields]
        [SerializeField] protected EffectConfig config;

        [SerializeField] protected int duration;
        [SerializeField] protected int elapsed;

        [SerializeField] protected int numStacks;
        #endregion

        #region Publics
        public EffectConfig Config => config;
        public int NumStacks => numStacks;
        public int Duration => duration;
        public int Elapsed => elapsed;

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

            elapsed++;
            UpdateEffect();
        }
        #endregion

        #region Internals
        protected abstract void ApplyEffect();
        protected abstract void EndEffect();
        protected abstract void UpdateEffect();

        protected Effect(EffectConfig config) {
            this.config = config;
            duration = 0;
            elapsed = 0;
            numStacks = 0;
        }
        #endregion
    }


    public abstract class Effect<T> : Effect where T : EffectConfig {

        public new T Config => config as T;

        protected Effect(T config) : base(config) {

        }
    }
}
