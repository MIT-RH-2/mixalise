using System.Collections.Generic;
using System.Linq;
using System.IO;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;

[RequireComponent(typeof(PrivilegeRequester))]
public class AudioRecorder : MonoBehaviour
{

    public GameObject vuMeter;

    [SerializeField, Tooltip("The audio source that should capture the microphone input.")]
    private AudioSource _inputAudioSource = null;

    [SerializeField, Tooltip("Controller connection handler")]
    private ControllerConnectionHandler _controllerConnectionHandler = null;
    
    private PrivilegeRequester _privilegeRequester = null;

    private bool _canCapture = false;
    private bool _isCapturing = false;
    private string _deviceMicrophone = string.Empty;

    private float _audioMaxSample = 0;
    private float[] _audioSamples = new float[128];

    private float _audioStart = 0;
    private float _audioEnd = 0;

    private const int AUDIO_CLIP_LENGTH_SECONDS = 3599;
    private const int AUDIO_CLIP_FREQUENCY_HERTZ = 48000;
    private const float AUDIO_SENSITVITY_DECIBEL = 0.00015f;

    // private const float AUDIO_CLIP_TIMEOUT_SECONDS = 2;
    // private const float AUDIO_CLIP_FALLOFF_SECONDS = 0.5f;
    // private const float ROTATION_DAMPING = 100;

    #region Unity Methods
    void Awake()
    {
        if (_inputAudioSource == null)
        {
            Debug.LogError("Error: AudioCaptureExample._inputAudioSource is not set, disabling script.");
            enabled = false;
            return;
        }

        // Before enabling the microphone, the scene must wait until the privileges have been granted.
        _privilegeRequester = GetComponent<PrivilegeRequester>();
        _privilegeRequester.OnPrivilegesDone += HandleOnPrivilegesDone;
        MLInput.OnControllerButtonDown += HandleOnButtonDown;

    }

    void OnDestroy()
    {
        _privilegeRequester.OnPrivilegesDone -= HandleOnPrivilegesDone;
        StopCapture();
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            if (_isCapturing)
            {
                // require privledges to be checked again.
                _canCapture = false;
                StopCapture();
            }
        }
    }

    void Update() {
        DetectAudio();
    }

    #endregion

    #region Private Methods
    private void StartCapture()
    {

        // Use the first detected Microphone device.
        if (Microphone.devices.Length > 0)
        {
            _deviceMicrophone = Microphone.devices[0];
        }

        // If no microphone is detected, exit early and log the error.
        if (string.IsNullOrEmpty(_deviceMicrophone))
        {
            Debug.LogError("Error: AudioCaptureExample._deviceMicrophone is not set.");
            return;
        }

        _isCapturing = true;
        _inputAudioSource.clip = Microphone.Start(_deviceMicrophone, true, AUDIO_CLIP_LENGTH_SECONDS, AUDIO_CLIP_FREQUENCY_HERTZ);
        _inputAudioSource.loop = true;

        // Delay to produce realtime playback effect.
        // while (!(Microphone.GetPosition(_deviceMicrophone) > 0)) { }

        _inputAudioSource.Play();
        _audioStart = _inputAudioSource.time;

        UpdateStatus();
    }

    private void StopCapture()
    {

        _audioEnd = _inputAudioSource.time;

        if (_isCapturing) {
            AudioClip clip = CreateAudioClip(_inputAudioSource.clip, _audioStart, _audioEnd);
            string path = Path.Combine(Application.persistentDataPath, "recordingZZZ.wav");
            AudioClipToWAV(clip, path);
        }

        _isCapturing = false;

        // Stop microphone and input audio source.
        _inputAudioSource.Stop();

        if (!string.IsNullOrEmpty(_deviceMicrophone))
        {
            Microphone.End(_deviceMicrophone);
        }

        UpdateStatus();
    }

    /// <summary>
    /// Update the example status label.
    /// </summary>
    private void UpdateStatus()
    {

        if (!_canCapture) {
            Debug.Log((_privilegeRequester.State != PrivilegeRequester.PrivilegeState.Failed) ? "Status: Requesting Privileges" : "Status: Privileges Denied");
        } else {
            Debug.Log("Status: capturing");
        }

    }

    private void DetectAudio()
    {
        // Analyze the input spectrum data, to determine when someone is speaking.
        _inputAudioSource.GetSpectrumData(_audioSamples, 0, FFTWindow.Rectangular);
        _audioMaxSample = Mathf.Lerp(_audioMaxSample, Mathf.Min(1, _audioSamples.Max() * 100), Time.deltaTime);

        vuMeter.transform.localRotation = Quaternion.Euler(0, 0, _audioMaxSample * 180 - 90);

    }

    /// <summary>
    /// Creates a new audio clip within the start and stop range.
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="start"></param>
    /// <param name="stop"></param>
    /// <returns></returns>
    private AudioClip CreateAudioClip(AudioClip clip, float start, float stop)
    {
        int length = (int)(clip.frequency * (stop - start));
        if (length <= 0)
        {
            return null;
        }

        AudioClip audioClip = AudioClip.Create("voice_track", length, 1, clip.frequency, false);

        float[] data = new float[length];
        clip.GetData(data, (int)(clip.frequency * start));
        audioClip.SetData(data, 0);

        return audioClip;
    }
    #endregion

    #region Event Handlers
    /// <summary>
    /// Responds to privilege requester result.
    /// </summary>
    /// <param name="result"/>
    private void HandleOnPrivilegesDone(MLResult result)
    {
        if (!result.IsOk)
        {
            if (result.Code == MLResultCode.PrivilegeDenied)
            {
                Instantiate(Resources.Load("PrivilegeDeniedError"));
            }

            Debug.LogErrorFormat("Error: AudioCaptureExample failed to get all requested privileges, disabling script. Reason: {0}", result);
            enabled = false;
            return;
        }

        _canCapture = true;
        Debug.Log("Succeeded in requesting all privileges");

        UpdateStatus();
    }

    private void HandleOnButtonDown(byte controllerId, MLInputControllerButton button)
    {
        if (_controllerConnectionHandler.IsControllerValid(controllerId))
        {
            if (_canCapture && button == MLInputControllerButton.Bumper)
            {
                // Stop & Start to clear the previous mode.
                if (_isCapturing)
                {
                    Debug.Log("Stop capture");
                    StopCapture();
                } else
                {
                    Debug.Log("Start capture");
                    StartCapture();
                }
            }
        }
    }

    public static bool AudioClipToWAV(AudioClip audio, string path)
    {
        return SavWav.Save(path, audio);
    }

    #endregion
}
