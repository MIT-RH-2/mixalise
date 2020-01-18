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

    public float TimeoutSeconds = 2f;

    public MLHandKeyPose Pose = MLHandKeyPose.Ok;
    public MLHandType handType;

    public float MinConfidence = 0.9f;

    public PoseEvent onPoseStartDetected;
    public PoseEvent onPoseCompleteDetected;
    public PoseEvent onPoseEndDetected;

    private IEnumerator poseTimeout;
    private MLHand hand;
    private Coroutine timeout;

    void Start() {

        if (handType == MLHandType.Left) {
            hand = MLHands.Left;
        } else {
            hand = MLHands.Right;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (hand == null) {
            return;
        }
        
        if (hand.KeyPose == Pose && hand.KeyPoseConfidence >= MinConfidence) {
            if (this.timeout == null) {
                this.timeout = StartCoroutine(this.PoseDetected());
            }
        } else {

            if (this.timeout != null) {
                StopCoroutine(this.timeout);
                onPoseEndDetected.Invoke(new PoseData(PoseData.PoseAction.Ended, this.Pose));
                this.timeout = null;
            }
        }

    }

    IEnumerator PoseDetected() {

        PoseData data = new PoseData(PoseData.PoseAction.Started, this.Pose);
        onPoseStartDetected.Invoke(data);

        yield return new WaitForSeconds(TimeoutSeconds);

        data = new PoseData(PoseData.PoseAction.Completed, this.Pose);
        onPoseCompleteDetected.Invoke(data);

    }
}