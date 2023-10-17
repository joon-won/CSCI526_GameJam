using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SendToGoogle : MonoBehaviour
{
    [SerializeField] private string URL = "https://docs.google.com/forms/u/1/d/e/1FAIpQLSdyEfEjgfi6jwA637_tKHjXJ4l-cbKpT6CQZbLaSgxsnSE4Fg/formResponse";

    private IEnumerator Post(string s1, string s2)
    {
        WWWForm form = new WWWForm(); 
        form.AddField("entry.1508416077", s1);
        form.AddField("entry.1659350666", s2);

        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }

    }

    public void Send(string a, string b)
    {
        StartCoroutine(Post(a, b));
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}