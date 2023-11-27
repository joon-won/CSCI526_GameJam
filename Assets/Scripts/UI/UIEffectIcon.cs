using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

namespace CSCI526GameJam {
    public class UIEffectIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

        #region Fields
        [SerializeField] private float animDuration;
        [SerializeField] private float bounceScale;

        [SerializeField] private RectTransform infoAnchor;
        [SerializeField] private Image icon;
        [SerializeField] private TMPro.TMP_Text stackText;

        private Effect effect;
        private Tween animTween;
        #endregion

        #region Publics
        public void Show(Effect effect) {
            this.effect = effect;

            RefreshInfo();
            transform.localScale = Vector3.zero;

            animTween = transform.DOScale(1f, animDuration).SetEase(Ease.OutBack);
        }

        public void Refresh() {
            RefreshInfo();

            animTween = transform.DOScale(bounceScale, animDuration * 0.5f).SetEase(Ease.OutQuad)
                .OnComplete(() => transform.DOScale(1f, animDuration * 0.5f).SetEase(Ease.InQuad));
        }

        public void Dispose() {
            Destroy(gameObject);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            UIPopups.Instance.EffectInfo.Show(effect, infoAnchor);
        }

        public void OnPointerExit(PointerEventData eventData) {
            UIPopups.Instance.EffectInfo.Hide();
        }
        #endregion

        #region Internals
        private void RefreshInfo() {
            icon.sprite = effect.Config.Image;
            icon.color = effect.Config.Tint;
            stackText.text = effect.NumStacks.ToString();
        }
        #endregion

        #region Unity Methods
        private void OnDisable() {
            UIPopups.Instance.EffectInfo.Hide();
        }
        #endregion
    }
}
