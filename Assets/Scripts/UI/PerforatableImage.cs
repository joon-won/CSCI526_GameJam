using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace CSCI526GameJam {
    public class PerforatableImage : Image {

        #region Fields
        [SerializeField] private List<UICutout> cutouts = new();
        private bool allowCutoutRaycast = true;
        #endregion

        #region Publics
        public bool AllowCutoutRaycast {
            get {
                return allowCutoutRaycast;
            }
            set {
                allowCutoutRaycast = value;
            }
        }

        public void AddCutout(UICutout cutout) {
            cutouts.Add(cutout);
        }

        public void AddCutouts(IEnumerable<UICutout> cutouts) {
            this.cutouts.AddRange(cutouts);
        }

        public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera) {
            if (allowCutoutRaycast && cutouts.Any(
                cutout => cutout.gameObject.activeInHierarchy 
                && IsScreenPointInCutout((RectTransform)cutout.transform, screenPoint, eventCamera))) {
                return false;
            }
            return base.IsRaycastLocationValid(screenPoint, eventCamera);
        }

        #endregion

        #region Internals
        private bool IsScreenPointInCutout(RectTransform cutout, Vector2 screenPoint, Camera eventCamera) {
            return RectTransformUtility.ScreenPointToLocalPointInRectangle(cutout, screenPoint, eventCamera, out var local)
                && cutout.rect.Contains(local);
        }
        #endregion

        #region Unity Methods
        #endregion
    }
}
