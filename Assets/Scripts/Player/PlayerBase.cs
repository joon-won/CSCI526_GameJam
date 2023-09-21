using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {
    public class PlayerBase : MonoBehaviour {

        #region Fields
        [MandatoryFields]
        [SerializeField] private float maxHealth;

        [ComputedFields]
        [SerializeField] private float health;

        #endregion

        #region Publics
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        private void OnTriggerEnter2D(Collider2D collision) {
            Debug.Log($"TODO: Enemy hits base. Health: ");   
        }
        #endregion
    }
}
