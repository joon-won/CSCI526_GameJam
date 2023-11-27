using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

namespace CSCI526GameJam {

    
    public class TowerStats {
        [ShowInInspector] public ModifierSet AttackDamage { get; } = new();
        [ShowInInspector] public ModifierSet AttackSpeed { get; } = new();
        [ShowInInspector] public ModifierSet AttackRange { get; } = new();
        [ShowInInspector] public ModifierSet CritChance { get; } = new();
        [ShowInInspector] public ModifierSet CritDamage { get; } = new();
    }

    public class EnemyStats {
        [ShowInInspector] public ModifierSet MaxHealth { get; } = new();
        [ShowInInspector] public ModifierSet Damage { get; } = new();
        [ShowInInspector] public ModifierSet MoveSpeed { get; } = new();
        [ShowInInspector] public ModifierSet Armor { get; } = new();
        [ShowInInspector] public ModifierSet Gold { get; } = new();
    }
    
    /// <summary>
    /// Contains all stats in the game. 
    /// </summary>

    public class StatSystem : MonoBehaviourSingleton<StatSystem> {

        #region Fields
        #endregion

        #region Publics
        [ShowInInspector] public TowerStats Tower { get; } = new();
        [ShowInInspector] public EnemyStats Enemy { get; } = new();
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
