using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CSCI526GameJam
{
    public class MainMenu : MonoBehaviour
    {
        public void GameRule()
        {
            SceneManager.LoadScene("Rule");
        }
    }
}
