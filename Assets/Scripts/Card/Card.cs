using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {

    [Serializable]
    public class Card {

        public enum Level {
            One = 0,
            Two = 1,
            Three = 2,
        }

        #region Fields
        [SerializeField] private CardConfig config;
        [SerializeField] private Level level;
        [SerializeField] private bool isCostHalved;
        #endregion

        #region Publics
        public event Action OnLevelChanged;

        public CardConfig Config => config;
        public string Name => config.CardName;
        public bool IsCostHalved => isCostHalved;
        public int Cost => isCostHalved ? config.Cost / 2 : config.Cost;
        public Sprite Image => config.Image;
        public Level CurrentLevel => level;

        public Card(CardConfig config) {
            this.config = config;
        }

        public void Play() {
            switch (level) {
                case Level.One:
                    config.PlayLv1();
                    break;

                case Level.Two:
                    config.PlayLv2();
                    break;

                case Level.Three:
                    config.PlayLv3();
                    break;
            }
        }

        public string GetDescription() {
            return config.Descriptions[(int)level];
        }

        public void SetLevel(Level level) {
            this.level = level;
            OnLevelChanged?.Invoke();
        }

        public void HalveCost() {
            isCostHalved = true;
        }

        public void Reset() {
            level = Level.One;
            isCostHalved = false;
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
