using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {

    public class Wall : Tower {

        #region Fields
        #endregion

        #region Publics
        #endregion

        #region Internals
        protected override bool CanAttack() {
            return false;
        }

        protected override void PerformAttack() {
            return;
        }

        protected override void PerformUpdate() {
            return;
        }
        #endregion

        #region Unity Methods
        #endregion
    }
}
