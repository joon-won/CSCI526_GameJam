using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Transform = UnityEngine.Transform;

namespace CSCI526GameJam {
    public class GameplayUI : MonoBehaviour {
        
        #region Fields
        #endregion

        #region Publics
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        private void Awake() {
            var initables = GetComponentsInChildren<IInitializable>(true);
            initables.ForEach(x => {
                x.Init();
            });
        }
        #endregion
    }
}
