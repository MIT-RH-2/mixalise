using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class AppMain : MonoBehaviour
{

    public PoseListener poseListener;

    // Start is called before the first frame update
    void Start()
    {

        // https://www.reddit.com/r/magicleap/comments/9610hm/does_anyone_know_the_refresh_rate/
        Application.targetFrameRate = 60;

        if (!MLHands.IsStarted) {
            MLHands.Start();
        }

        if (poseListener != null) {
            poseListener.Init();
        }

    }

    void OnDestroy() {
        if (MLHands.IsStarted) {
            MLHands.Stop();
        }
    }
}
