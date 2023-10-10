using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CSCI526GameJam
{
    public class UIGameContinue : MonoBehaviour
    {

        // Update is called once per frame
        void Update()
        {
            if (Input.anyKeyDown)
            {
                // SceneManager.LoadScene("GamePlay");
                foreach (GameObject rootObject in SceneManager.GetSceneByName("GamePlay").GetRootGameObjects())
                {
                    rootObject.SetActive(true);
                }
                SceneManager.UnloadSceneAsync("GameContinue");
            }
        }
    }
}
