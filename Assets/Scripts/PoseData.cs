using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.MagicLeap;

public class PoseData {

    public enum PoseAction {
        Started,
        Completed
    }

    public PoseAction action { get; private set; }
    public MLHandKeyPose pose { get; private set; }

    public PoseData(PoseAction action, MLHandKeyPose pose) {
        this.action = action;
        this.pose = pose;
    }

}
