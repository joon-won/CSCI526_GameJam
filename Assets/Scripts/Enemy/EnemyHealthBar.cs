using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace CSCI526GameJam {
    public class EnemyHealthBar : MonoBehaviour {

        #region Fields
        [ClassHeader(typeof(EnemyHealthBar))]

        [MandatoryFields]
        [SerializeField] private Transform healthBar;

        [ComputedFields]
        [SerializeField] private Enemy enemy;
        #endregion

        #region Publics
        public void Show() {
            gameObject.SetActive(true);
        }

        public void Hide() {
            gameObject.SetActive(false);
        }

        public void SetPercentage(float percentage) {
            healthBar.localScale =
                new Vector3(percentage,
                healthBar.localScale.y,
                healthBar.localScale.z);
        }
        #endregion

        #region Internals
        private void Refresh() {
            var percentage = enemy.CurrentHitPoint / enemy.MaxHitPoint;
            SetPercentage(percentage);

            if (!enemy.IsAlive) {
                Hide();
            }
            else {
                Show();
            }
        }
        #endregion

        #region Unity Methods
        private void Awake() {
            enemy = GetComponentInParent<Enemy>();
            if (!enemy) {
                Debug.LogWarning($"No {typeof(Enemy)} is found in parent of {GetType()}");
            }
            enemy.OnHitPointChanged += (delta) => Refresh();
            Refresh();
        }
        #endregion
    }
}
