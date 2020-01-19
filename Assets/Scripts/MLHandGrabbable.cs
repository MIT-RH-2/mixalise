using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MLHandGrabbable : MonoBehaviour
{
    public UnityEvent onGrabStart;
    public UnityEvent onGrabEnd;

    private bool isKinematicAtGrab;
    private Vector3 grabOffset;
    private Transform startingParent;

    private void Start()
    {
        if (GetComponent<Rigidbody>() == null)
        {
            Debug.LogError("Grabbable needs a RigidBody");
        }

        startingParent = transform.parent;
    }

    public void StartGrab(MLHandGrabber grabber)
    {
        transform.SetParent(grabber.transform, true);

        var rigid = GetComponent<Rigidbody>();

        if (rigid != null)
        {
            isKinematicAtGrab = rigid.isKinematic;
            rigid.isKinematic = true;
        }

        onGrabStart.Invoke();
        //grabOffset = transform.position - grabber.transform.position;
    }

    public void StopGrab(MLHandGrabber grabber)
    {
        transform.SetParent(startingParent, true);

        var rigid = GetComponent<Rigidbody>();

        if (rigid != null)
        {
            rigid.isKinematic = isKinematicAtGrab;
        }

        onGrabEnd.Invoke();
    }

    public void UpdateGrab(MLHandGrabber grabber)
    {
        //transform.position = grabber.transform.position + grabOffset;
    }
}
