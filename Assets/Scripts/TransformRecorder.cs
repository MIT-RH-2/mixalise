using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

using System.Threading;
using UnityEngine;

public class TransformRecorder : MonoBehaviour
{

    [Serializable]
    private class TransformChange {
        public float time;

        public float px;
        public float py;
        public float pz;

        public float rx;
        public float ry;
        public float rz;
        public float rw;
    }

    public string UniqueID = null;

    private Queue<TransformChange> changes = new Queue<TransformChange>();

    private FileStream stream;
    private bool running = true;
    private Thread writeThread = null;

    void Start() {

        string path = Path.Combine(Application.persistentDataPath, $"{this.UniqueID}.xform");
        this.stream = File.Open(path, FileMode.OpenOrCreate);

        this.writeThread = new Thread(new ThreadStart(this.DoWriteTransforms));
        this.writeThread.Start();

    }

    void OnDestroy() {

        this.running = false;
        this.writeThread.Join();

        stream.Flush();
        stream.Close();
    }

    void Update()
    {
        if (this.transform.hasChanged) {

            lock(changes) {
                changes.Enqueue(new TransformChange() {

                    time = Time.time,

                    px = transform.position.x,
                    py = transform.position.y,
                    pz = transform.position.z,

                    rx = transform.rotation.x,
                    ry = transform.rotation.y,
                    rz = transform.rotation.z,
                    rw = transform.rotation.w

                });
            }

            this.transform.hasChanged = false;
        }
    }

    void OnValidate() {
        if (String.IsNullOrEmpty(UniqueID)) {
            UniqueID = System.Guid.NewGuid().ToString();
        }
    }

    void DoWriteTransforms() {

        BinaryFormatter formatter = new BinaryFormatter();

        while (this.running) {

            lock (this.changes) {
                while (this.changes.Count > 0) {
                    formatter.Serialize(stream, this.changes.Dequeue());
                }
            }

            Thread.Yield();
        }
    }
}
