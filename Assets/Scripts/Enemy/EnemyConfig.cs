using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam
{
    public class EnemyConfig : ScriptableObject
    {
        #region Fields
        [TextArea(5, 10)]
        [SerializeField] private string description;
        [SerializeField] private int golddrop;
        [SerializeField] private float hitPoint;
        [SerializeField] private float maxHitPoint;
        [SerializeField] private float attackDamage;        
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion        
    }
}
