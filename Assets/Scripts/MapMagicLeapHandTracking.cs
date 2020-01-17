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
            UpdateHand(MLHands.Left, targetLeftHand);
        }

        if (MLHands.Right != null)
        {
            UpdateHand(MLHands.Right, targetRightHand);
        }
    }

    void UpdateHand(MLHand hand, Transform target)
    {
        target.position = hand.Wrist.Center.Position;
    }
}
