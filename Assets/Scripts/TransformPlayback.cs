using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using UnityEngine;

public class TransformPlayback : MonoBehaviour
{
    
    TransformRecorder recorder;
    string UniqueID;
    Stream playbackStream;

    Thread updateThread;
    List<TransformChange> timeline;
    int currentFrameIdx = 0;
    TransformChange currentFrame;
    bool playing = false;

    void Start()
    {

        Debug.Log("StreamPlayback.Start()");

        recorder = GetComponent<TransformRecorder>();
        this.UniqueID = recorder.UniqueID;

        this.ReadPlayback();
        this.StartPlayback();

    }

    // Update is called once per frame
    void Update()
    {

        if (!this.playing) {
            return;
        }

        if (this.timeline == null) {
            return;
        }

        if (this.currentFrame == null) {
            return;
        }

        Vector3 pos = new Vector3(
            this.currentFrame.px,
            this.currentFrame.py,
            this.currentFrame.pz
        );

        Quaternion rot = new Quaternion(
            this.currentFrame.rx,
            this.currentFrame.ry,
            this.currentFrame.rz,
            this.currentFrame.rw
        );

        transform.position = pos;
        transform.rotation = rot;

    }

    void ReadPlayback() {

        BinaryFormatter formatter = new BinaryFormatter();
        List<TransformChange> _timeline = new List<TransformChange>();

        string fileName = recorder.GetFileName();
        if (File.Exists(fileName)) {
            Debug.Log($"Opening {fileName}");
        } else {
            Debug.LogError($"Could not find file {fileName}");
            return;
        }

        using (FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read)) {
            try {
                while (fs.Position < fs.Length) {
                    TransformChange change = formatter.Deserialize(fs) as TransformChange;
                    _timeline.Add(change);
                }
            } catch (Exception e) {
                Debug.Log($"Hurf: {e.Message}");
            }
        }

        Debug.Log($"{_timeline.Count} frames");

        this.timeline = _timeline;

    }

    public void StartPlayback() {
        this.playing = true;

        this.updateThread = new Thread(new ThreadStart(this.DoPlayback));
        this.updateThread.Start();
    }

    public void StopPlayback() {
        this.playing = false;

        this.updateThread.Join();
    }

    void DoPlayback() {
        this.currentFrameIdx = 0;

        while(this.playing) {
            this.currentFrame = this.timeline[this.currentFrameIdx];
            Thread.Sleep((int)this.currentFrame.time);
            this.currentFrameIdx++;
            this.currentFrameIdx = this.currentFrameIdx % this.timeline.Count;
        }
    }
}
