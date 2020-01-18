using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class MapMagicLeapHandTracking : MonoBehaviour
{
    public const float FPS_60 = 0.016666666666667f;

    public Transform targetLeftHand;
    public Transform targetRightHand;
    public float handRotationEase = 0.1f;
    public GameObject debugPrefab;

    private Transform debugHandStart;
    private Transform debugHandUp;

    private void Start()
    {
        MLHands.Start();

        if (debugPrefab != null)
        {
            var deubgScale = 0.1f;
            var debugBallScale = new Vector3(deubgScale, deubgScale, deubgScale);

            var startDebug = Instantiate(debugPrefab);
            startDebug.name = "startDebug";

            var upDebug = Instantiate(debugPrefab);
            upDebug.name = "upDebug";

            debugHandStart = startDebug.transform;
            debugHandUp = upDebug.transform;

            debugHandStart.localScale = debugBallScale;
            debugHandUp.localScale = debugBallScale;
        }
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

        var a = hand.Wrist.Center.Position;
        var b = hand.Index.KeyPoints[0].Position;
        var c = hand.Middle.KeyPoints[0].Position;

        var aToB = b - a;
        var aToC = c - a;
        var centerToCenter = hand.Wrist.Center.Position - a;
        var handUp = Vector3.Cross(aToB, aToC).normalized;

        //var rotation = Quaternion.LookRotation(handUp, Vector3.up);
        var rotation = Quaternion.LookRotation(a - b, handUp);
        target.rotation = Quaternion.Slerp(target.rotation, rotation, handRotationEase);

        if (debugHandStart)
        {
            debugHandStart.position = a;
            debugHandUp.position = a + handUp;

            Debug.DrawLine(debugHandStart.position, debugHandUp.position, new Color(1, 0, 0, 1));
        }
        
    }
}
