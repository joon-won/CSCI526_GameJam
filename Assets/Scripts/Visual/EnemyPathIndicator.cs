using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace CSCI526GameJam {

    public class EnemyPathIndicator : MonoBehaviour {

        #region Fields
        [MandatoryFields]
        [SerializeField] private float spacing;

        [SerializeField] private Gradient groundColor;
        [SerializeField] private Gradient airColor;
        [SerializeField] private PathArrow groundArrowPrefab;
        [SerializeField] private PathArrow airArrowPrefab;

        private List<PathArrow> arrows = new();
        private LineRenderer lineRenderer;
        #endregion

        #region Publics
        public void SetPath(Path path, bool isAirPath = false) {
            if (path.GroundSpots.Count <= 1 || path.AirSpots.Count <= 1) return;

            if (spacing <= 0f) {
                Debug.LogWarning($"Spacing must be positive. ");
                return;
            }

            var prefab = isAirPath ? airArrowPrefab : groundArrowPrefab;
            var spots = isAirPath ? path.AirSpots : path.GroundSpots;
            var offset = isAirPath ? 1 : 0;

            var positions = spots.Select(x => x.Position).ToArray();
            lineRenderer.positionCount = positions.Length;
            lineRenderer.SetPositions(positions);
            lineRenderer.colorGradient = isAirPath ? airColor : groundColor;
            lineRenderer.sortingOrder = isAirPath ? 1 : 0; // Air is over ground

            var length = 0f;
            for (int i = 0; i < spots.Count - 1; i++) {
                var spot = spots[i];
                length += Vector3.Distance(spots[i + 1].Position, spot.Position);
            }
            var numArrows = Mathf.RoundToInt(length / spacing);
            var realSpacing = length / numArrows;

            var spotIndex = 0;
            var remainingSpacing = realSpacing;
            if (isAirPath) {
                remainingSpacing *= 0.5f;
            }
            var arrowIndex = 0;
            while (arrowIndex < numArrows) {

                var currPos = spots[spotIndex].Position;
                var nextPos = spots[spotIndex + 1].Position;

                var distance = Vector3.Distance(currPos, nextPos);
                if (distance < remainingSpacing) {
                    remainingSpacing -= distance;
                    spotIndex++;
                    spotIndex = spotIndex % (spots.Count - 1);
                    continue;
                }

                var arrow = Instantiate(prefab, transform);
                arrow.transform.position = Vector3.Lerp(currPos, nextPos, remainingSpacing / distance);
                arrow.Init(positions, spotIndex);

                remainingSpacing += realSpacing;
                arrowIndex++;
            }


            //var first = spots[0].Position;
            //var second = spots[1].Position;
            //var dist = Vector3.Distance(first, second);
            //if (dist < remainingSpacing) return;

            //var arr = Instantiate(prefab, transform);
            //arr.transform.position = Vector3.Lerp(second, first, remainingSpacing / dist);
            //arr.Init(positions, spotIndex);

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
