using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace CSCI526GameJam {
    public class AnalyticsManager : MonoBehaviourSingleton<AnalyticsManager> {

        [SerializeField] private bool sendInEditor;

        [SerializeField] private string URL =
            "https://docs.google.com/forms/u/1/d/e/1FAIpQLSdyEfEjgfi6jwA637_tKHjXJ4l-cbKpT6CQZbLaSgxsnSE4Fg/formResponse";

        private IEnumerator Post(string s1, string s2) {
            WWWForm form = new WWWForm();
            form.AddField("entry.1508416077", s1);
            form.AddField("entry.1659350666", s2);

            using (UnityWebRequest www = UnityWebRequest.Post(URL, form)) {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success) {
                    Debug.Log(www.error);
                }
                else {
                    Debug.Log("Form upload complete!");
                }
            }
        }

        private void Send() {
            if (!sendInEditor && Application.isEditor) return;

            var level = GameManager.Instance.Level.ToString();
            var numTowers = TowerManager.Instance.NumTowers.ToString();
            StartCoroutine(Post(level, numTowers));
        }

        private void Start() {
            GameManager.Instance.OnCombatStarted += Send;
        }
    }
}