using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace CSCI526GameJam {
    public class UICutout : MonoBehaviour {

        #region Fields
        [MandatoryFields]
        [SerializeField] private float startScale;

        [SerializeField] private float prepareRatio;

        [SerializeField] private Image border;
        [SerializeField] private RippleEffect ripple1;
        [SerializeField] private RippleEffect ripple2;

        private Color borderColor;
        private Sequence showSeq;
        #endregion

        #region Publics
        public void Show(float animDuration, Ease easeMode) {
            gameObject.SetActive(true);

            transform.localScale = Vector3.one * startScale;

            var color = borderColor;
            color.a = 0f;
            border.color = color;

            showSeq.Kill();
            showSeq = DOTween.Sequence();

            showSeq.Join(transform.DOScale(Vector3.one, animDuration)
                .SetEase(easeMode)
                .SetDelay(animDuration * prepareRatio));

            showSeq.Join(border.DOFade(1f, animDuration * prepareRatio)
                .SetEase(easeMode));

            ripple1.Show(easeMode);
            ripple2.Show(easeMode);
        }

        public void Hide() {
            gameObject.SetActive(false);
            showSeq.Kill();
            ripple1.Hide();
            ripple2.Hide();
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        private void Awake() {
            GetComponent<Image>().raycastTarget = false;
            borderColor = border.color;
        }
        #endregion
    }
}
