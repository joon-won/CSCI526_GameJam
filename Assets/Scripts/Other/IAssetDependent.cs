using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAssetDependent {

#if UNITY_EDITOR
    void FindAssets();
#endif
}

