using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class XRMap
{
    public Transform xrTarget;
    public Transform rigTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    public void Map()
    {
        rigTarget.position = xrTarget.TransformPoint(trackingPositionOffset);
        rigTarget.rotation = xrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}

public class XRRig : MonoBehaviour
{
    public XRMap head;
    public XRMap leftArm;
    public XRMap rightArm;
    public Transform headConstraint;

    private Vector3 headBodyOffset;

    // Start is called before the first frame update
    void Start()
    {
        headBodyOffset = transform.position - headConstraint.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = headConstraint.position + headBodyOffset;
        transform.forward = Vector3.ProjectOnPlane(headConstraint.up, Vector3.up).normalized;

        head.Map();
        leftArm.Map();
        rightArm.Map();
    }
}
