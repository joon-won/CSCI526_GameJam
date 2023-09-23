using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam
{
    public class Enemy : MonoBehaviour
    {

        #region Fields
        [SerializeField] protected EnemyConfig config;
        [SerializeField] protected Numeric attackDamage;
        [SerializeField] protected Numeric attackSpeed;
        [SerializeField] private Numeric attackRange;        
        [SerializeField] private Numeric moveSpeed;
        [SerializeField] private Numeric hitPoint;
        [SerializeField] private Numeric hpMod;
        [SerializeField] private Numeric barrier;
        [SerializeField] private float attackCooldown = 0f;
        #endregion

        #region Public
        public Action onDeath;

        #endregion
        public bool IsDead()
        {
            return hitPoint.Value <= 0f;
        }

        public void RemoveDeadEnemy()
        {
            if (IsDead())
            {
                Destroy(gameObject);
                onDeath?.Invoke();
            }
        }

        public void CalculateRealHP()
        {                                    
            hitPoint = new(100f);
            hpMod = new(1f);
            barrier = new(0f);

            hitPoint.IncreaseScalar(hpMod);
            hitPoint.IncreaseValue(barrier);            
        }

        public void TakeDamage(float damage) 
        {                        
            hitPoint.IncreaseValue(-1 * barrier);
        }

        // Start is called before the first frame update
        void Start()
        {
            CalculateRealHP();            
        }

        // Update is called once per frame
        void Update()
        {
            RemoveDeadEnemy();
        }
    }
}
