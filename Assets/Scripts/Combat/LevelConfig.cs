using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {

    [Serializable]
    public struct LevelInfo {
        public WaveInfo[] WaveInfos;
        public int NumWaves => WaveInfos.Length;
    }

    [Serializable]
    public struct WaveInfo {
        public EnemyConfig[] Enemies;
        public int NumEnemies => Enemies.Length;
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
