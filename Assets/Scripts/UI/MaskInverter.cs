using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace CSCI526GameJam {

    [AddComponentMenu("DreamCode/UI/Mask Inverter")]
    public sealed class MaskInverter : MonoBehaviour, IMaterialModifier {

        #region Fields
        private static readonly int _stencilComp = Shader.PropertyToID("_StencilComp");
        #endregion

        #region Publics
        public Material GetModifiedMaterial(Material baseMaterial) {
            var resultMaterial = new Material(baseMaterial);
            resultMaterial.SetFloat(_stencilComp, Convert.ToSingle(CompareFunction.NotEqual));
            return resultMaterial;
        }
        #endregion

        #region Internals
        #endregion

        #region Unity Methods
        #endregion
    }
}
