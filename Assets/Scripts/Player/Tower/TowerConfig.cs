using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CSCI526GameJam {

    [CreateAssetMenu(menuName = "Config/Tower/General")]
    public class TowerConfig : ScriptableObject {

        #region Fields
        [SerializeField] private Sprite image;
        [TextArea(5, 10)]
        [SerializeField] private string description;

        [SerializeField] private float attackDamage;
        [SerializeField] private float attackSpeed;
        [SerializeField] private float attackRange;
        [SerializeField] private float critChance;
        [SerializeField] private float critDamage;
        #endregion

        #region Publics
        public Sprite Image => image;
        public string Description => description;

        public float AttackDamage => attackDamage;
        public float AttackSpeed => attackSpeed;
        public float AttackRange => attackRange;
        public float CritChance => critChance;
        public float CritDamage => critDamage;
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
