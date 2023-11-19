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
        #endregion

        #region Publics
        public CardConfig Config => config;
        public int Cost => config.Cost;

        public Card(CardConfig config) {
            this.config = config;
        }

        public void Play(Level level) {
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

        public string GetDescription(Level level) {
            return config.Descriptions[(int)level];
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
