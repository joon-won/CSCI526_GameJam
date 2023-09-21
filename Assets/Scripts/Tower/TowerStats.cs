using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {
    public class TowerStats {

        #region Fields
        #endregion

        #region Publics
        [ShowInInspector] public NumericSet AttackDamage { get; } = new();
        [ShowInInspector] public NumericSet AttackSpeed { get; } = new();
        [ShowInInspector] public NumericSet AttackRange { get; } = new();
        [ShowInInspector] public NumericSet CritChance { get; } = new();
        [ShowInInspector] public NumericSet CritDamage { get; } = new();
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
