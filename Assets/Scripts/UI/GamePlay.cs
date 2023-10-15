using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace CSCI526GameJam
{
    public class GamePlay : MonoBehaviour
    {   
        // change scene from rule scene to gamePlay scene
       public void GameStart()
        {
            SceneManager.LoadScene("GamePlay");
        }
    }
}
