using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

namespace CSCI526GameJam {
    public class EffectManager : MonoBehaviourSingleton<EffectManager> {

        #region Fields
        [ComputedFields]
        [SerializeField] private List<EffectConfig> orderedConfigs = new();
        [ShowInInspector] private Dictionary<EffectConfig, Effect> effects = new();
        #endregion

        #region Publics
        public event Action<Effect> OnEffectAdded;
        public event Action<Effect[]> OnEffectUpdated;
        public event Action<Effect[]> OnEffectRemoved;

        /// <summary>
        /// Add a new effect. 
        /// </summary>
        public void AddEffect(Effect effect) {
            if (effects.TryGetValue(effect.Config, out var existingEffect)) {
                existingEffect.Apply();
                OnEffectAdded?.Invoke(existingEffect);
            }
            else {
                orderedConfigs.Add(effect.Config);
                effects[effect.Config] = effect;
                effect.Apply();
                OnEffectAdded?.Invoke(effect);
            }
        }
        #endregion

        #region Internals
        private void UpdateAll() {
            var updated = new List<Effect>();
            var removed = new List<Effect>();
            foreach (var effect in effects.Values.ToList()) {
                effect.Update();
                if (effect.IsExpired) {
                    effect.End();
                    orderedConfigs.Remove(effect.Config);
                    effects.Remove(effect.Config);

                    removed.Add(effect);
                }
                else {
                    updated.Add(effect);
                }
            }

            OnEffectUpdated?.Invoke(updated.ToArray());
            OnEffectRemoved?.Invoke(removed.ToArray());
        }
        #endregion

        #region Unity Methods
        protected override void Awake() {
            base.Awake();

            GameManager.Instance.OnPreparationStarted += UpdateAll;
            GameManager.Instance.OnCurrentSceneExiting += () => {
                GameManager.Instance.OnPreparationStarted -= UpdateAll;
            };
        }
        #endregion
    }
}
