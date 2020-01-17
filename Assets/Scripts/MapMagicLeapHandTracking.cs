using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class MapMagicLeapHandTracking : MonoBehaviour
{
    public Transform TargetLeftHand;
    public Transform TargetRightHand;

    private void Start()
    {
        MLHands.Start();
    }

    void Update()
    {
        var leftWrist = MLHands.Left.Wrist;
        var rightWrist = MLHands.Right.Wrist;

        TargetLeftHand.position = leftWrist.Center.Position;
        TargetRightHand.position = rightWrist.Center.Position;
    }

    void OnDestroy()
    {
        MLHands.Stop();
    }
}
