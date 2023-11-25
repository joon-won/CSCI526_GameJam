using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CSCI526GameJam {
    public class PerforatableImage : Image {

        #region Fields
        [SerializeField] private List<RectTransform> cutouts = new();
        #endregion

        #region Publics
        public void AddCutout(RectTransform cutout) {
            cutouts.Add(cutout);
        }

        public void AddCutouts(IEnumerable<RectTransform> cutouts) {
            this.cutouts.AddRange(cutouts);
        }

        public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera) {

            foreach (var cutout in cutouts) {
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(cutout, screenPoint, eventCamera, out var local)) {
                    if (cutout.rect.Contains(local)) {
                        return false;
                    }
                }
            }

            return base.IsRaycastLocationValid(screenPoint, eventCamera);
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
