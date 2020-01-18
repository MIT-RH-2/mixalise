using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreamPlayback : MonoBehaviour
{

    private static StreamPlayback _instance;

    public static StreamPlayback Instance {
        get {
            return _instance;
        }
    }

    void Awake() {
        if (_instance != null) {
            Debug.LogError("Only once instance of StreamPlayback allowed");
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
    }

}
