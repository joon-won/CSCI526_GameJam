using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {

    public class UIPerforatableMask : MonoBehaviour {

        #region Fields
        [MandatoryFields]
        [SerializeField] private Transform cutoutHolder;
        [SerializeField] private PerforatableImage perforatableImage;
        #endregion

        #region Publics
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        private void Awake() {
            List<RectTransform> cutouts = new();
            foreach (RectTransform cutout in cutoutHolder.transform) {
                cutouts.Add(cutout);
            }
            perforatableImage.AddCutouts(cutouts);
        }
        #endregion
    }
}
