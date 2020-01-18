using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class StreamRecorder : MonoBehaviour
{

    private static StreamRecorder _instance;

    public static StreamRecorder Instance {
        get {
            return _instance;
        }
    }

    private List<TransformRecorder> transformRecorders = new List<TransformRecorder>();
    private List<string> recordedObjects = new List<string>();
    private AudioRecorder audioRecorder;

    private float startTime;
    private float stopTime;

    public string RemoteIp;

    [Range(1025, 65535)]
    public int RemotePort;

    void Awake() {
        if (_instance != null) {
            Debug.LogError("Only once instance of StreamRecorder allowed");
            return;
        }

        _instance = this;
    }

    public void StartRecording() {
        this.startTime = Time.time;
        this.audioRecorder.StartCapture();
        this.transformRecorders.ForEach(x => x.StartRecording());
    }

    public void StopRecording() {
        this.stopTime = Time.time;
        this.audioRecorder.StopCapture();
        this.transformRecorders.ForEach(x => x.StopRecording());
    }

    public void SubmitRecording() {

        List<string> files = new List<string>();
        files.Add(this.audioRecorder.GetAudioFile());
        files.AddRange(this.transformRecorders.Select(x => x.GetFileName()));

        // TODO send files over network

    }

    public void AddTransformRecorder(TransformRecorder recorder) {
        if (this.transformRecorders.Contains(recorder)) {
            this.transformRecorders.Add(recorder);
            this.recordedObjects.Add(recorder.UniqueID);
        }
    }

    public void RemoveTransformRecorder(TransformRecorder recorder) {
        this.transformRecorders.Remove(recorder);
    }

    public void SetAudioRecorder(AudioRecorder recorder) {
        this.audioRecorder = recorder;
    }
}
