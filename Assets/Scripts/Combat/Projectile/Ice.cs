using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {

    public class Ice : Projectile {

        #region Fields
        [ClassHeader(typeof(Ice))]

        [MandatoryFields]
        [SerializeField] private Vector2 scaleRange;
        [SerializeField] private float fadingThreshold = 0.8f;

        [ComputedFields]
        [SerializeField] private float speed;
        [SerializeField] private float elapsed = 0f;
        [SerializeField] private float duration;

        private SpriteRenderer spriteRenderer;
        #endregion

        #region Publics

        /// <summary>
        /// Fire the flame. 
        /// </summary>
        /// <param name="direction">Shooting direction. </param>
        /// <param name="speed">Bullet speed. </param>
        /// <param name="range">Max shooting distance. </param>
        public void Fire(Vector3 direction, float speed, float range) {
            transform.FaceTo(direction.normalized);
            this.speed = speed;
            duration = range / speed;
            elapsed = 0f;
            transform.localScale = Vector3.one * scaleRange.x;
            spriteRenderer.color = Color.white;
        }
        #endregion

        #region Internals
        protected override void OnDisposed() {
            damage = 0f;
            speed = 0f;
            elapsed = 0f;
            duration = 0f;
        }
        #endregion

        #region Unity Methods
        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update() {
            if (elapsed > duration) {
                Dispose();
                return;
            }
            elapsed += Time.deltaTime;
            transform.position += transform.up * speed * Time.deltaTime;

            var progress = elapsed / duration;
            transform.localScale = Vector3.one * Mathf.Lerp(scaleRange.x, scaleRange.y, progress);

            if (progress > fadingThreshold) {
                var color = Color.white;
                color.a = Mathf.Lerp(1f, 0f, (progress - fadingThreshold) / (1f - fadingThreshold));
                spriteRenderer.color = color;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            var target = collision.GetComponent<Enemy>();
            if (!target) return;

            target.TakeDamage(damage);
            target.FreezeEntity(1f);
            //Debug.Log("TODO: Freeze enemy");
        }
        #endregion
    }
}
