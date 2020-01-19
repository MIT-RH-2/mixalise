using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class AppMain : MonoBehaviour
{

    public List<PoseListener> poseListeners;

    // Start is called before the first frame update
    void Start()
    {

        // https://www.reddit.com/r/magicleap/comments/9610hm/does_anyone_know_the_refresh_rate/
        Application.targetFrameRate = 60;

        if (!MLHands.IsStarted) {
            MLHands.Start();
        }

        poseListeners.ForEach(x => x.Init());

    }

    void OnDestroy() {
        if (MLHands.IsStarted) {
            MLHands.Stop();
        }
    }
}
