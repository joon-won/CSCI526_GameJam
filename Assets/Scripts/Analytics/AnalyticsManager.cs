using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace CSCI526GameJam {
    public class AnalyticsManager : MonoBehaviourSingleton<AnalyticsManager> {

        [SerializeField] private bool sendInEditor;

        [SerializeField] private string URL =
            "https://docs.google.com/forms/u/1/d/e/1FAIpQLSdyEfEjgfi6jwA637_tKHjXJ4l-cbKpT6CQZbLaSgxsnSE4Fg/formResponse";

        private long sessionID;

        private bool checkNext = false;

        private IEnumerator Post(string s1, string s2, string s3, string s4, string s5, string s6, string s7) {
            WWWForm form = new WWWForm();
            form.AddField("entry.1594772069", s1);
            form.AddField("entry.1508416077", s2);
            form.AddField("entry.1659350666", s3);
            form.AddField("entry.130137863", s4);
            form.AddField("entry.1234595824", s5);
            form.AddField("entry.791766504", s6);
            form.AddField("entry.778273508", s7);

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

            int level = GameManager.Instance.Level;
            var numTowers = TowerManager.Instance.NumTowers.ToString();
            var gold = Player.Instance.Gold.ToString();
            var baseHealth = TowerManager.Instance.PlayerBase.Health.ToString();
            var cardUsage = GetCardUsage();
            var state = GameManager.Instance.GameState.ToString();
            if (!state.Contains("Combat"))
            {
                level++;
                checkNext = true;
            }
            if(checkNext && level == 1)
            {
                // New sessionID if we restart the game
                sessionID = System.DateTime.Now.Ticks;
            }
            
            StartCoroutine(Post(sessionID.ToString(), level.ToString(), numTowers, gold, baseHealth, cardUsage, state));
        }

        string GetCardUsage()
        {
            // Get the usage per card in JSON Format
            var cardUsage = CardManager.Instance.CardConfigToUsageNum;
            var entries = cardUsage.Select(d =>
                string.Format("\"{0}\": {1}", d.Key.CardName.ToString(), string.Join(",", d.Value.ToString())));
            return "{" + string.Join(",", entries) + "}";
        }

        public new void Awake()
        {
            base.Awake();
            // Assign sessionID to identify playtests
            sessionID = System.DateTime.Now.Ticks;
            DontDestroyOnLoad(gameObject);
        }

        private void Start() {
            // To Store the game logs at the start of these 3 stages
            GameManager.Instance.OnCombatStarted += Send;
            GameManager.Instance.OnGameOver += Send;
            GameManager.Instance.OnGameWon += Send;
        }
    }
}