using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {
    public class EffectManager : MonoBehaviourSingleton<EffectManager> {

        #region Fields
        [ComputedFields]
        [SerializeField] private List<Effect> persists = new();
        [SerializeField] private List<Effect> temps = new();
        #endregion

        #region Publics
        /// <summary>
        /// Add a new effect. 
        /// </summary>
        /// <param name="onAdd">Effect on added. </param>
        /// <param name="onRemove">Effect on removed. </param>
        /// <param name="onUpdate">Effect on updated. </param>
        /// <param name="duration">Num of rounds to persist. Permanent if duration is negative. </param>
        public void AddEffect(Action onAdd, Action onRemove, Action onUpdate = null, int duration = -1) {
            var effect = new Effect(onAdd, onRemove, onUpdate, duration);
            effect.Add();

            if (duration < 0) persists.Add(effect);
            else temps.Add(effect);
        }
        #endregion

        #region Internals
        private void UpdateAllTemps() {
            for (int i = 0; i < temps.Count; i++) {
                var effect = temps[i];
                effect.Update();
                if (!effect.IsExpired) continue;

                effect.Remove();
                temps.RemoveAt(i);
                i--;
            }
        }
        #endregion

        #region Unity Methods
        private void OnEnable() {
            GameManager.Instance.OnPreparationStarted += UpdateAllTemps;
        }

        private void OnDisable() {
            //if (!GameManager.IsApplicationQuitting) {
            //    GameManager.Instance.OnPreparationStarted -= UpdateAllTemps;
            //}
        }
        #endregion

        [Serializable]
        private class Effect {

            #region Fields
            [ClassHeader(typeof(Effect))]

            [ComputedFields]
            [SerializeField] private int elapsed;
            [SerializeField] private int duration; // persistent if negative
            private Action onAdd;
            private Action onRemove;
            private Action onUpdate;
            #endregion

            #region Publics
            public float Duration => duration;
            public bool IsExpired => duration > 0 && elapsed >= duration;

            public Effect(Action onAdd, Action onRemove, Action onUpdate, int duration) {
                this.onAdd = onAdd;
                this.onRemove = onRemove;
                this.onUpdate = onUpdate;
                this.duration = duration;
                elapsed = 0;
            }

            public void Add() {
                onAdd?.Invoke();
            }

            public void Remove() {
                onRemove?.Invoke();
            }

            public void Update() {
                if (duration > 0 && elapsed < duration) {
                    elapsed++;
                }
                onUpdate?.Invoke();
            }
            #endregion

            #region Internals
            #endregion

            #region Unity Methods
            #endregion
        }
    }
}
