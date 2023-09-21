using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSCI526GameJam {

    public enum Direction {
        None = -1,
        Top,
        Bottom,
        Left,
        Right,
    }

    //public enum ItemRank {
    //    Common,
    //    Rare,
    //    Lengendary,
    //}


    public static class Configs {
        //public const string Version = "0.1.0";

        //public const string SaveDirectory = "Save";
        //public const string SaveFileName = "profiledata";
        //public const string SaveExtension = ".sav";
        //public const int ProfileMaxCount = 3;
        //public const string EncryptionPassword = "";

        public const int MainMenuSceneIndex = 0;
        public const int GameplaySceneIndex = 1;

        public const float CellSize = 1f;
    }
}
