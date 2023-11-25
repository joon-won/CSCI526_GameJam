using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace CSCI526GameJam {

    [RequireComponent(typeof(CanvasGroup))]
    public class UIMoverOnCombat : MonoBehaviour {

        #region Fields
        [MandatoryFields]
        [SerializeField] private Vector2 stepOffset;
        [SerializeField] private float stepDuration;

        [ComputedFields]
        [SerializeField] private Vector2 origin;

        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;
        #endregion

        #region Publics
        #endregion

        #region Internals
        private void OnCombatStartedHandler() {
            canvasGroup.interactable = false;

            var pos = rectTransform.anchoredPosition + stepOffset;
            rectTransform.DOAnchorPos(pos, stepDuration).SetEase(Ease.OutQuad);
        }

        [ContextMenu("t")]
        private void OnPreparationStartedHandler() {
            rectTransform.DOAnchorPos(origin, stepDuration).SetEase(Ease.OutQuad)
                .OnComplete(() => canvasGroup.interactable = true);
        }
        #endregion

        #region Unity Methods
        private void Start() {
            canvasGroup = GetComponent<CanvasGroup>();

            rectTransform = transform as RectTransform;
            origin = rectTransform.anchoredPosition;

            GameManager.Instance.OnCombatStarted += OnCombatStartedHandler;
            GameManager.Instance.OnPreparationStarted += OnPreparationStartedHandler;
            GameManager.Instance.OnCurrentSceneExiting += () => {
                GameManager.Instance.OnCombatStarted -= OnCombatStartedHandler;
                GameManager.Instance.OnPreparationStarted -= OnPreparationStartedHandler;
            };
        }
        #endregion
    }
}
