using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CSCI526GameJam
{
    public class HelpPanel : MonoBehaviour
    {
        public void GameStart()
        {
            SceneManager.UnloadSceneAsync("HelpPanel");
        }
    }
}
