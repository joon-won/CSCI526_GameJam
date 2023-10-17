
#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class RefreshAssetDependents {

    #region Fields
    #endregion

    #region Publics
    [MenuItem("Tools/Refresh All AssetDependents")]
    public static void Refresh() {
        var ads = new List<Object>();

        var objs = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var obj in objs) {
            var components = obj.GetComponentsInChildren<Component>();
            foreach (var c in components) {
                if (c is IAssetDependent) {
                    ads.Add(c);
                }
            }
        }

        foreach (var ad in ads) {
            ((IAssetDependent)ad).FindAssets();
            EditorUtility.SetDirty(ad);
        }
    }
    #endregion

    #region Internals
    #endregion

    #region Unity Methods
    #endregion
}
#endif
