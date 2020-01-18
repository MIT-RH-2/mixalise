using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.MagicLeap;

[Serializable]
public class PoseEvent : UnityEvent<PoseData>
{

}

public class PoseListener : MonoBehaviour
{

    private Coroutine timeout;

    public float TimeoutSeconds = 2f;

    public MLHandKeyPose Pose = MLHandKeyPose.Ok;
    public MLHand hand;

    public float MinConfidence = 0.9f;

    IEnumerator poseTimeout;

    // Update is called once per frame
    void Update()
    {
        
        if (hand.KeyPose == Pose && hand.KeyPoseConfidence >= MinConfidence) {
            this.timeout = StartCoroutine(this.PoseDetected());
        } else {
            StopCoroutine(this.timeout);
        }

    }

    IEnumerator PoseDetected() {

        PoseData data = new PoseData(PoseData.PoseAction.Started, this.Pose);
        new PoseEvent().Invoke(data);

        yield return new WaitForSeconds(TimeoutSeconds);

        data = new PoseData(PoseData.PoseAction.Completed, this.Pose);
        new PoseEvent().Invoke(data);

    }
}