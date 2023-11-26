using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

namespace CSCI526GameJam {

    public class UIPerforatableMask : MonoBehaviour, IPointerClickHandler {

        #region Fields
        [MandatoryFields]
        [SerializeField] private float animDuration;
        [SerializeField] private Ease easeMode;
        [SerializeField] private Color shadeColor;

        [SerializeField] private Transform cutoutHolder;
        [SerializeField] private PerforatableImage perforatableImage;

        private List<UICutout> cutouts = new();
        private Tween showTween;
        #endregion

        #region Publics
        public event Action<PointerEventData> OnClicked;


        public void Show() {
            gameObject.SetActive(true);
            perforatableImage.color = Color.clear;

            showTween.Kill();
            showTween = perforatableImage.DOColor(shadeColor, animDuration).SetEase(easeMode);

            cutouts.ForEach(x => x.Show(animDuration, easeMode));
        }

        public void Hide() {
            gameObject.SetActive(false);
            showTween.Kill();
            cutouts.ForEach(x => x.Hide());
        }

        public void ToggleCutoutRaycast(bool isEnabled) {
            perforatableImage.AllowCutoutRaycast = isEnabled;
        }

        public void OnPointerClick(PointerEventData eventData) {
            OnClicked?.Invoke(eventData);
        }

        public void ClearOnClickedEvents() {
            OnClicked = null;
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        private void Awake() {
            foreach (RectTransform cutout in cutoutHolder.transform) {
                cutouts.Add(cutout.GetComponent<UICutout>());
            }
            perforatableImage.AddCutouts(cutouts);
        }
        #endregion
    }
}
