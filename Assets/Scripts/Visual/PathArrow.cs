using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {
    public class PathArrow : MonoBehaviour {

        #region Fields
        [MandatoryFields]
        [SerializeField] private int offset;
        [SerializeField] private float speed;

        [ComputedFields]
        [SerializeField] private int index;

        private Vector3[] waypoints;
        private float distance;
        #endregion

        #region Publics
        public void Init(Vector3[] positions, int currIndex) {
            waypoints = positions;

            index = currIndex + 1;
            if (index >= waypoints.Length) {
                index = 1;
                transform.position = waypoints[0];
            }
            else {
                //transform.position = positions[currIndex];
            }
            distance = Vector3.Distance(waypoints[index], transform.position);
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        private void Update() {
            var targetPos = waypoints[index];

            var dir = (targetPos - waypoints[index - 1]).normalized;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            var step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
            distance -= step;

            if (distance <= 0f) {
                index++;
                if (index >= waypoints.Length) {
                    index = 1;
                    transform.position = waypoints[0];
                }

                // Handle overflowed step. 
                transform.position = Vector3.MoveTowards(transform.position, waypoints[index], -distance);
                distance += Vector3.Distance(waypoints[index], waypoints[index - 1]);
            }
        }
        #endregion
    }
}
