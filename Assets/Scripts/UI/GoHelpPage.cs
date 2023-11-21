using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CSCI526GameJam
{
    public class GoHelpPage : MonoBehaviour
    {
        public void HelpPage()
        {
            SceneManager.LoadScene("HelpPanel",LoadSceneMode.Additive);
        }
    }
}