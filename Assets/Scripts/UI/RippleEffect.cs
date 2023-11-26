using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace CSCI526GameJam {
    public class RippleEffect : MonoBehaviour {

        #region Fields
        [MandatoryFields]
        [SerializeField] private float repeatInterval;
        [SerializeField] private float animDuration;
        [SerializeField] private float delay;
        [SerializeField] private float startScale;

        [SerializeField] private Image ripple;

        [ComputedFields]
        [SerializeField] private Ease easeMode;
        [SerializeField] private bool isRepeating;
        [SerializeField] private float showElapsed;

        private Color borderColor;
        private Sequence showSeq;
        #endregion

        #region Publics
        public void Show(Ease easeMode, bool isRepeating = true) {
            this.easeMode = easeMode;
            this.isRepeating = isRepeating;

            gameObject.SetActive(true);

            transform.localScale = Vector3.one * startScale;

            var color = borderColor;
            color.a = 0f;
            ripple.color = color;

            showSeq.Kill();
            showSeq = DOTween.Sequence();

            showSeq.PrependInterval(delay);
            showSeq.Join(transform.DOScale(Vector3.one, animDuration)
                .SetEase(easeMode)
                .SetDelay(animDuration));

            showSeq.Join(ripple.DOFade(1f, animDuration)
                .SetEase(easeMode));

            showSeq.Append(ripple.DOFade(0f, animDuration)
                .SetEase(easeMode));
        }

        public void Hide() {
            gameObject.SetActive(false);
            showSeq.Kill();
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        private void Awake() {
            borderColor = ripple.color;
        }

        private void Update() {
            if (isRepeating) {
                if (showElapsed < repeatInterval) {
                    showElapsed += Time.deltaTime;
                    return;
                }
                showElapsed = 0f;
                Show(easeMode);
            }
        }
        #endregion
    }
}
