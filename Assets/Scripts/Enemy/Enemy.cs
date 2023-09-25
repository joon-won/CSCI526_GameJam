using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam
{
    public abstract class Enemy : MonoBehaviour
    {

        #region Fields
        [SerializeField] protected EnemyConfig config;
        [SerializeField] protected Numeric attackDamage;
        [SerializeField] private Numeric moveSpeed;
        [SerializeField] private Numeric hitPoint;
        [SerializeReference] private Numeric armor;
        [SerializeField] protected bool isAlive = true;
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

            hitPoint.IncreaseScalar(1f);            
        }

        public void TakeDamage(float damage) 
        {                        
            hitPoint.IncreaseValue(-1 * damage);

            if (IsDead())
            {
                isAlive = false;
                Destroy(gameObject);
                onDeath?.Invoke();
            }            
        }

        // Start is called before the first frame update
        void Start()
        {
            CalculateRealHP();            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
