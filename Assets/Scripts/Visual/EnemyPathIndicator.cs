using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace CSCI526GameJam {

    [RequireComponent(typeof(LineRenderer))]
    public class EnemyPathIndicator : MonoBehaviour {

        #region Fields
        private LineRenderer lineRenderer;
        #endregion

        #region Publics
        public void SetPath(Path path) {
            var positions = path.Spots.Select(x => x.Position).ToArray();
            lineRenderer.positionCount = positions.Length;
            lineRenderer.SetPositions(positions);
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        private void Awake() {
            lineRenderer = GetComponent<LineRenderer>();
        }
        #endregion
    }
}
