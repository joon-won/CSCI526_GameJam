
#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class RefreshAssetDependents {

    #region Fields
    #endregion

    #region Publics
    [MenuItem("Tools/Refresh all AssetDependents")]
    public static void Refresh() {
        var ads = Utility.FindAllInActiveScene<IAssetDependent>();
        foreach (var ad in ads) {
            ad.FindAssets();
        }
    }
    #endregion

    #region Internals
    #endregion

    #region Unity Methods
    #endregion
}
#endif
