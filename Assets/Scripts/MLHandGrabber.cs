using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;
using UnityEngine.Events;

[System.Serializable]
public class EventGrabUpdate : UnityEvent<bool> { }

public class MLHandGrabber : MonoBehaviour
{
    public bool isGrabbing { get; private set; }
    public MLHandType handType = MLHandType.Left;
    public MLHandKeyPose grabPose = MLHandKeyPose.Fist;
    public float minConfidence = 0.9f;
    public EventGrabUpdate onGrabUpdate;

    private MLHand hand;
    private List<MLHandGrabbable> collidingGrabables = new List<MLHandGrabbable>();
    private List<MLHandGrabbable> movingGrabbables;

    private void Start()
    {
        var curCollider = GetComponent<Collider>();

        if (curCollider == null)
        {
            Debug.LogError("Grabber should have a collider");
        }

        curCollider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(handType == MLHandType.Left)
        {
            hand = MLHands.Left;
        } else
        {
            hand = MLHands.Right;
        }

        CheckGrabGesture();

        if (isGrabbing)
        {
            movingGrabbables.ForEach((grabable) => { grabable.UpdateGrab(this); });
        }
    }

    void CheckGrabGesture()
    {
        if (hand == null)
        {
            return;
        }

        if (!hand.IsVisible)
        {
            return;
        }

        var isDoingGrabGesture = hand.KeyPose == grabPose;

        if (isDoingGrabGesture != isGrabbing)
        {
            isGrabbing = isDoingGrabGesture;
            onGrabUpdate.Invoke(isGrabbing);

            if (isGrabbing)
            {
                movingGrabbables = new List<MLHandGrabbable>(collidingGrabables);
                movingGrabbables.ForEach((grabable) => { grabable.StartGrab(this); });
            }
            else
            {
                movingGrabbables.ForEach((grabable) => { grabable.StopGrab(this); });
                movingGrabbables = null;
            }
        }
    }

    void OnTriggerEnter(Collider collidingItem)
    {
        var grabbable = collidingItem.gameObject.GetComponent<MLHandGrabbable>();

        if (grabbable == null)
        {
            return;
        }

        collidingGrabables.Add(grabbable);
    }

    void OnTriggerExit(Collider collidingItem)
    {
        var grabbable = collidingItem.gameObject.GetComponent<MLHandGrabbable>();

        if (grabbable == null)
        {
            return;
        }

        collidingGrabables.Remove(grabbable);
    }
}
