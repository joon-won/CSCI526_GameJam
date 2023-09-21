using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

namespace CSCI526GameJam {

    /// <summary>
    /// Contains all stats in the game. 
    /// </summary>

    public class Stats : MonoBehaviourSingleton<Stats> {

        #region Fields
        //[MandatoryFields]
        //[SerializeField] private PlayerConfig playerConfig;
        #endregion

        #region Publics
        //[ShowInInspector] public PlayerStats Player { get; } = new();

        [ShowInInspector] public TowerStats Tower { get; } = new();
        //[ShowInInspector] public EnemyStats Enemy { get; } = new();
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        protected override void Awake() {
            base.Awake();
            //Player.Init(playerConfig);
        }
        #endregion
    }
}
