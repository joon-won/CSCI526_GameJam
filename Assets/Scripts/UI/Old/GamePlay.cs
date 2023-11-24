using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace CSCI526GameJam
{
    public class GamePlay : MonoBehaviour
    {   
        public void GameStart()
        {
            SceneManager.LoadScene("GamePlay");
        }
    }
}
