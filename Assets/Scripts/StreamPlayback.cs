using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StreamPlayback : MonoBehaviour
{

    private static StreamPlayback _instance;

    public static StreamPlayback Instance {
        get {
            return _instance;
        }
    }

    public AudioSource AudioContainer;
    private Coroutine playAudio;

    void Awake() {
        if (_instance != null) {
            Debug.LogError("Only once instance of StreamPlayback allowed");
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
    }

    void Start() {
        string path = "file://" + Path.Combine(Application.persistentDataPath, "recording.wav");

        playAudio = StartCoroutine(LoadAudio());

    }

    void OnDestroy() {
        if (playAudio != null) {
            AudioContainer.Stop();
            StopCoroutine(playAudio);
        }
    }

    IEnumerator LoadAudio() {

        string path = "file:///" + Path.Combine(Application.persistentDataPath, "recording.wav");
        WWW url = new WWW(path);

        yield return url;

        AudioContainer.clip = url.GetAudioClip(false, true);
        AudioContainer.Play();

    }

}
