using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CSCI526GameJam {

    public class UIPerforatableMask : MonoBehaviour, IPointerClickHandler {

        #region Fields
        [MandatoryFields]
        [SerializeField] private Transform cutoutHolder;
        [SerializeField] private PerforatableImage perforatableImage;
        #endregion

        #region Publics
        public event Action<PointerEventData> OnClicked;

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
            List<RectTransform> cutouts = new();
            foreach (RectTransform cutout in cutoutHolder.transform) {
                cutout.GetComponent<Image>().raycastTarget = false;
                cutouts.Add(cutout);
            }
            perforatableImage.AddCutouts(cutouts);
        }
        #endregion
    }
}
