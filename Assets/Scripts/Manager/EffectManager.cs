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
        /// <summary>
        /// Add a new effect. 
        /// </summary>
        public void AddEffect(Effect effect) {
            if (effects.TryGetValue(effect.Config, out var existingBuff)) {
                existingBuff.Apply();
            }
            else {
                orderedConfigs.Add(effect.Config);
                effects[effect.Config] = effect;
                effect.Apply();
            }
        }
        #endregion

        #region Internals
        private void UpdateAll() {
            foreach (var effect in effects.Values.ToList()) {
                effect.Update();
                if (!effect.IsExpired) continue;

                effect.End();
                orderedConfigs.Remove(effect.Config);
                effects.Remove(effect.Config);
            }
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
