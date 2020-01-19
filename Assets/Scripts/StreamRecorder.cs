using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;

public class StreamRecorder : MonoBehaviour
{
    public UnityEvent onStartRecord;
    public UnityEvent onStopRecord;

    private static StreamRecorder _instance;

    public static StreamRecorder Instance {
        get {
            return _instance;
        }
    }

    private List<TransformRecorder> transformRecorders = new List<TransformRecorder>();
    private List<string> recordedObjects = new List<string>();
    public AudioRecorder audioRecorder;

    public bool recording = false;

    private float startTime;
    private float stopTime;

    public string RemoteIp;

    [Range(1025, 65535)]
    public int RemotePort;

    void Awake() {
        if (_instance != null) {
            Debug.LogError("Only once instance of StreamRecorder allowed");
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
    }

    public void OnDestroy() {
        StopRecording();
    }

    public void ToggleRecording() {
        if (this.recording) {
            this.StopRecording();
        } else {
            this.StartRecording();
        }
    }

    public void StartRecording() {

        this.recording = true;

        this.startTime = Time.time;
        this.transformRecorders.ForEach(x => x.StartRecording());
        Debug.Log("Recording");
        
        if (this.audioRecorder != null) {
            this.audioRecorder.StartCapture();
        }

        onStartRecord.Invoke();
    }

    public void StopRecording() {

        this.recording = false;

        this.stopTime = Time.time;
        this.transformRecorders.ForEach(x => x.StopRecording());
        Debug.Log("Stop Recording");

        if (this.audioRecorder != null) {
            this.audioRecorder.StopCapture();
        }

        onStopRecord.Invoke();
    }

    public void SubmitRecording() {

        List<string> files = new List<string>();
        files.Add(this.audioRecorder.GetAudioFile());
        files.AddRange(this.transformRecorders.Select(x => x.GetFileName()));

        // TODO send files over network

    }

    public void AddTransformRecorder(TransformRecorder recorder) {

        if (this.recordedObjects.Contains(recorder.recordingId.id) && ! this.transformRecorders.Contains(recorder)) {
            Debug.LogError($"Duplicate TransformRecorder ID detected: {recorder.recordingId.id}");
            return;
        }

        if (!this.transformRecorders.Contains(recorder)) {
            this.transformRecorders.Add(recorder);
            this.recordedObjects.Add(recorder.recordingId.id);
        }

    }

    public void RemoveTransformRecorder(TransformRecorder recorder) {
        this.transformRecorders.Remove(recorder);
    }

    public void SetAudioRecorder(AudioRecorder recorder) {
        this.audioRecorder = recorder;
    }
}
