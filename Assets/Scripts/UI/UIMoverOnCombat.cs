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

        private Vector3 origin;
        #endregion

        #region Publics
        #endregion

        #region Internals
        private void OnCombatStartedHandler() {
            transform.DOMove(transform.position + stepOffset, stepDuration).SetEase(Ease.OutQuad);
        }

        private void OnPreparationStartedHandler() {
            transform.DOMove(origin, stepDuration).SetEase(Ease.OutQuad);
        }
        #endregion

        #region Unity Methods
        private void Awake() {
            origin = transform.position;

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
