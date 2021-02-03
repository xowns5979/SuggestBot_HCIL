using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using System.Linq;

using UnityEngine.Networking;

public class Test : MonoBehaviour
{
    string url = "https://i.pinimg.com/564x/bc/72/07/bc72075040c3b529b0ab442bc0203f99.jpg";
    System.Diagnostics.Stopwatch sw;


    KeywordRecognizer keywordRecognizer;
    delegate void KeywordAction(PhraseRecognizedEventArgs args);
    Dictionary<string, KeywordAction> keywordCollection;

    // Start is called before the first frame update
    void Start()
    {
        sw = new System.Diagnostics.Stopwatch();

        keywordCollection = new Dictionary<string, KeywordAction>();
        keywordCollection.Add("Get Texture", GetTexture_);
        keywordRecognizer = new KeywordRecognizer(keywordCollection.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }
    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        KeywordAction keywordAction;
        if (keywordCollection.TryGetValue(args.text, out keywordAction))
            keywordAction.Invoke(args);
    }
    void GetTexture_(PhraseRecognizedEventArgs prea)
    {
        StartCoroutine(GetTexture());
    }

    IEnumerator GetTexture()
    {
        sw.Reset();
        sw.Start();
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        Texture myTexture = DownloadHandlerTexture.GetContent(www);
        GameObject.Find("TestCanvas/RawImage").GetComponent<RawImage>().texture = myTexture;

        Debug.Log("GetTexture Time: " + sw.ElapsedMilliseconds + "ms");
        sw.Stop();
    }


    // Update is called once per frame
    void Update()
    {

    }


 
}
