using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;

namespace CSCI526GameJam {

    [Serializable]
    public struct LevelInfo {
        public WaveInfo[] WaveInfos;
        public int NumWaves => WaveInfos.Length;

        [ShowInInspector]
        public int MaxGold => WaveInfos.Sum(
            waveInfo => waveInfo.EnemyInfos.Sum(x => {
                if (!x.Config) return 0;
                return x.Config.GoldDrop * x.Num;
            }));
    }

    [Serializable]
    public struct WaveInfo {
        public EnemyInfo[] EnemyInfos;
        public int NumEnemies => EnemyInfos.Sum(x => x.Num);
    }

    [Serializable]
    public struct EnemyInfo {
        public EnemyConfig Config;
        public int Num;
    }

    [CreateAssetMenu(menuName = "Config/Level")]
    public class LevelConfig : ScriptableObject {

        #region Fields
        [SerializeField] private LevelInfo[] levelInfos;
        #endregion

        #region Publics
        public LevelInfo[] LevelInfos => levelInfos;
        public int NumLevels => levelInfos.Length;
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
