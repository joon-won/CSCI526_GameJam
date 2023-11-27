using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {
    public class DamageFlasher : MonoBehaviour {

        private static readonly int flashAmountID = Shader.PropertyToID("_FlashAmount");
        private static readonly int flashColorID = Shader.PropertyToID("_FlashColor");
        
        #region Fields
        [SerializeField] private Color color = Color.white;
        [SerializeField] private float duration = 0.15f;

        private Coroutine flashRoutine;
        private Material material;
        #endregion

        #region Publics
        public void Flash() {
            if (flashRoutine != null) {
                StopCoroutine(flashRoutine);
            }

            flashRoutine = StartCoroutine(FlashRoutine());
        }
        #endregion

        #region Internals
        private IEnumerator FlashRoutine() {
            var elapsed = 0f;
            while (elapsed < duration) {
                var amount = Mathf.Lerp(1f, 0f, elapsed / duration);
                material.SetFloat(flashAmountID, amount);

                yield return new WaitForSeconds(Time.deltaTime);
                elapsed += Time.deltaTime;
            }

            flashRoutine = null;
        }

        private void Awake() {
            material = GetComponent<SpriteRenderer>().material;
            material.SetColor(flashColorID, color);
        }

        private void OnEnable() {
            material.SetFloat(flashAmountID, 0f);
        }
        #endregion
    }
}
