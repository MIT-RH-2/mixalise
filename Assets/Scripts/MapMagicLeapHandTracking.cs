using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class MapMagicLeapHandTracking : MonoBehaviour
{
    public Transform targetLeftHand;
    public Transform targetRightHand;

    private void Start()
    {
        MLHands.Start();
    }

    void OnDestroy()
    {
        MLHands.Stop();
    }

    void Update()
    {
        if (MLHands.Left != null)
        {
            Debug.Log("left" + MLHands.Left.Index.KeyPoints[0].Position);
            UpdateHand(MLHands.Left, targetLeftHand);
        }

        if (MLHands.Right != null)
        {
            Debug.Log("right" + MLHands.Right.Index.KeyPoints[0].Position);
            UpdateHand(MLHands.Right, targetRightHand);
        }
    }

    void UpdateHand(MLHand hand, Transform target)
    {
        target.position = hand.Wrist.Center.Position;

        var centerToUlnar = hand.Wrist.Ulnar.Position - hand.Center;
        var centerToRadial = hand.Wrist.Radial.Position - hand.Center;
        var centerToCenter = hand.Wrist.Center.Position - hand.Center;
        var handUp = Vector3.Cross(centerToUlnar, centerToRadial);

        var rotation = Quaternion.LookRotation(centerToCenter, handUp);
        target.rotation = rotation;
    }
}
