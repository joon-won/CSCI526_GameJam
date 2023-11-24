using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace CSCI526GameJam {
    public class UIMoverOnCombat : MonoBehaviour {

        #region Fields
        [MandatoryFields]
        [SerializeField] private Vector3 stepOffset;
        [SerializeField] private float stepDuration;

        [ComputedFields]
        [SerializeField] private Vector3 origin;

        private RectTransform rectTransform;
        #endregion

        #region Publics
        #endregion

        #region Internals
        private void OnCombatStartedHandler() {
            rectTransform.DOMove(rectTransform.position + stepOffset, stepDuration).SetEase(Ease.OutQuad);
        }

        private void OnPreparationStartedHandler() {
            rectTransform.DOMove(origin, stepDuration).SetEase(Ease.OutQuad);
        }
        #endregion

        #region Unity Methods
        private void Start() {
            rectTransform = transform as RectTransform;
            origin = rectTransform.position;

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
