using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLHandGrabbable : MonoBehaviour
{
    private bool isKinematicAtGrab;
    private Vector3 grabOffset;

    private void Start()
    {
        if (GetComponent<Rigidbody>() == null)
        {
            Debug.LogError("Grabbable needs a RigidBody");
        }
    }

    public void StartGrab(MLHandGrabber grabber)
    {
        var rigid = GetComponent<Rigidbody>();

        if (rigid != null)
        {
            isKinematicAtGrab = rigid.isKinematic;
            rigid.isKinematic = true;
        }

        grabOffset = transform.position - grabber.transform.position;
    }

    public void StopGrab(MLHandGrabber grabber)
    {
        var rigid = GetComponent<Rigidbody>();

        if (rigid != null)
        {
            rigid.isKinematic = isKinematicAtGrab;
        }
    }

    public void UpdateGrab(MLHandGrabber grabber)
    {
        transform.position = grabber.transform.position + grabOffset;
    }
}
