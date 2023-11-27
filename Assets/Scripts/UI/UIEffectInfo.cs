using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CSCI526GameJam {
    public class UIEffectInfo : MonoBehaviour {

        #region Fields
        [MandatoryFields]
        [SerializeField] private UIEffectIcon iconPrefab;

        [ComputedFields]
        [ShowInInspector] private Dictionary<EffectConfig, UIEffectIcon> configToIcon = new();
        #endregion

        #region Publics
        #endregion

        #region Internals
        private void OnAddedHandler(Effect effect) {
            if (!configToIcon.TryGetValue(effect.Config, out var icon)) {
                icon = Instantiate(iconPrefab, transform);
                configToIcon[effect.Config] = icon;
            }
            icon.Show(effect);
        }

        private void OnUpdatedHandler(Effect[] effects) {
            effects.ForEach(effect => {
                if (!configToIcon.TryGetValue(effect.Config, out var icon)) {
                    Debug.LogWarning($"Icon of {effect.Config.name} does not exist. ");
                    return;
                }
                icon.Refresh();
            });
        }

        private void OnRemovedHandler(Effect[] effects) {
            effects.ForEach(effect => {
                if (!configToIcon.TryGetValue(effect.Config, out var icon)) {
                    Debug.LogWarning($"Icon of {effect.Config.name} does not exist. ");
                    return;
                }
                icon.Dispose();
                configToIcon.Remove(effect.Config);
            });
        }
        #endregion

        #region Unity Methods
        private void Awake() {
            EffectManager.Instance.OnEffectAdded += OnAddedHandler;
            EffectManager.Instance.OnEffectUpdated += OnUpdatedHandler;
            EffectManager.Instance.OnEffectRemoved += OnRemovedHandler;
        }
        #endregion
    }
}
