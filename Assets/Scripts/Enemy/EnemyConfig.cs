using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {
    [CreateAssetMenu(menuName = "Config/Enemy/General")]
    public class EnemyConfig : ScriptableObject
    {
        #region Fields
        [SerializeField] private Sprite regularSprite;
        [SerializeField] private Sprite frozenSprite;
        [TextArea(5, 10)]
        [SerializeField] private string description;

        [SerializeField] private int golddrop;
        [SerializeField] private float maxHitPoint;
        [SerializeField] private float attackDamage;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float armor;
        [SerializeField] private bool isFlying;
        #endregion

        #region Publics
        public Sprite RegularSprite => regularSprite;
        public Sprite FrozenSprite => frozenSprite;
        public string Description => description;
        public int GoldDrop => golddrop;
        public float MaxHitPoint => maxHitPoint;
        public float AttackDamage => attackDamage;
        public float MoveSpeed => moveSpeed;
        public float Armor => armor;
        public bool IsFlying => isFlying;
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
