using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using System.Threading;
using UnityEngine;

public class TransformRecorder : MonoBehaviour
{
	public RecordingIDSCriptableObject recordingId;

	private Queue<TransformChange> changes = new Queue<TransformChange>();

    private FileStream stream;
    private bool running = false;
    private Thread writeThread = null;

    void Start() {
        if (recordingId == null)
		{
			Debug.LogError("Recording ID is not defined");
		}

        if (StreamRecorder.Instance != null) {
            StreamRecorder.Instance.AddTransformRecorder(this);
        }

        if (StreamPlayback.Instance != null) {

            Debug.Log("Playback mode");

            TransformPlayback playback = GetComponent<TransformPlayback>();
            if (playback == null) {
                playback = gameObject.AddComponent(typeof(TransformPlayback)) as TransformPlayback;
            }
        }
    }

    void OnDestroy() {
        this.StopRecording();

        if (StreamRecorder.Instance != null) {
            StreamRecorder.Instance.RemoveTransformRecorder(this);
        }
    }

	void Update()
	{

		if (!this.running)
		{
			return;
		}

        lock (changes)
        {
            changes.Enqueue(new TransformChange()
            {

                time = Time.deltaTime,

                px = transform.position.x,
                py = transform.position.y,
                pz = transform.position.z,

                rx = transform.rotation.x,
                ry = transform.rotation.y,
                rz = transform.rotation.z,
                rw = transform.rotation.w

            });
        }

	}

    public void StartRecording() {

        this.running = true;

        string path = this.GetFileName();
        this.stream = File.Open(path, FileMode.OpenOrCreate);

        this.writeThread = new Thread(new ThreadStart(this.DoWriteTransforms));
        this.writeThread.Start();

    }

    public void StopRecording() {
        this.running = false;

        if (this.writeThread != null) {
            this.writeThread.Join();
            this.writeThread = null;
        }

        if (this.stream != null) {
            this.stream.Flush();
            this.stream.Close();
            this.stream = null;
            Debug.Log($"Wrote {GetFileName()}");
        }

    }

    void DoWriteTransforms() {

        BinaryFormatter formatter = new BinaryFormatter();

        while (this.running) {

            lock (this.changes) {
                while (this.changes.Count > 0) {
                    formatter.Serialize(this.stream, this.changes.Dequeue());
                }
            }

            Thread.Yield();
        }
    }

    public string GetFileName() {
        return Path.Combine(Application.persistentDataPath, $"{this.recordingId.id}.xform");
    }
}
