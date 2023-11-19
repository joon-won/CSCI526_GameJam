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


    public static class Configs {
        public const string Version = "0.0.1";

        public const int MainMenuSceneIndex = 0;
        public const int GameplaySceneIndex = 1;

        public const float CellSize = 1f;
    }
}
