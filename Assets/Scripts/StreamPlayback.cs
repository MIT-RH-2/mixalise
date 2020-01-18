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

        StartCoroutine(LoadAudio());

    }

    IEnumerator LoadAudio() {

        string path = "file:///" + Path.Combine(Application.persistentDataPath, "recording.wav");
        WWW url = new WWW(path);

        yield return url;

        AudioContainer.clip = url.GetAudioClip(false, true);
        AudioContainer.Play();

    }

}
