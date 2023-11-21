using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CSCI526GameJam {
    public class UIQuitGame : MonoBehaviour {
        public void QuitGame() {
            ApplicationHelper.Quit();
            Debug.Log("Quit Game!");
        }
    }
}
