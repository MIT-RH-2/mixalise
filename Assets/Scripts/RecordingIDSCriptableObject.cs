using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecordingID", menuName = "Youtube XR Creator/Recorder", order = 1)]
public class RecordingIDSCriptableObject : ScriptableObject
{
    public string id = System.Guid.NewGuid().ToString();
}
