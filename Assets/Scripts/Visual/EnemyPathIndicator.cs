using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace CSCI526GameJam {

    [RequireComponent(typeof(LineRenderer))]
    public class EnemyPathIndicator : MonoBehaviour {

        #region Fields
        [SerializeField] private Gradient groundColor;
        [SerializeField] private Gradient airColor;

        private LineRenderer lineRenderer;
        #endregion

        #region Publics
        public void SetPath(Path path, bool isAirPath = false) {
            var spots = isAirPath ? path.AirSpots : path.GroundSpots;
            var positions = spots.Select(x => x.Position).ToArray();
            lineRenderer.positionCount = positions.Length;
            lineRenderer.SetPositions(positions);
            lineRenderer.colorGradient = isAirPath ? airColor : groundColor;
            lineRenderer.sortingOrder = isAirPath ? 1 : 0; // Air is over ground
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
