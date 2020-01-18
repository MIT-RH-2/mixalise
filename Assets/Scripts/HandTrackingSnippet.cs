using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class HandTrackingSnippet : MonoBehaviour
{
	public GameObject prefab;
	public Material material;

	private List<MLKeyPoint> _keypoints = new List<MLKeyPoint>();
	private List<GameObject> _trackingPoints = new List<GameObject>();
	private List<MLHand> _hands = new List<MLHand>();
	private float _keyPoseConfidence = 0.9f;

	// List of the KeyPoses we will try to detect.
	private MLHandKeyPose[] _keyPoses = { MLHandKeyPose.Fist,MLHandKeyPose.C,
										  MLHandKeyPose.L, MLHandKeyPose.Pinch,
										  MLHandKeyPose.Ok };

	void Start()
	{
		// Start Hands
		MLHands.Start();

		// List with left and Right hands
		_hands.Add(MLHands.Left);
		_hands.Add(MLHands.Right);

		// Filter parameters.
		MLHands.KeyPoseManager.SetKeyPointsFilterLevel(MLKeyPointFilterLevel.ExtraSmoothed);
		MLHands.KeyPoseManager.SetPoseFilterLevel(MLPoseFilterLevel.ExtraRobust);

		// Enable keypose detection
		MLHands.KeyPoseManager.EnableKeyPoses(_keyPoses, true, true);

		// Setup the keypoints and prefab spheres to visualize them.
		Setup();
	}

	void OnDestroy()
	{
		// Stop Hands
		MLHands.Stop();
	}

	void Update()
	{
		// Update keypoint positions
		for (var i = 0; i < _keypoints.Count; ++i)
		{
			_trackingPoints[i].transform.localPosition = _keypoints[i].Position;
		}

		// Change the material color when keyPose on the list is detected
		material.color = Color.cyan;
		foreach (MLHand hand in _hands)
		{
			if (hand.KeyPoseConfidence > _keyPoseConfidence)
			{
				foreach (MLHandKeyPose keyPose in _keyPoses)
				{
					if (hand.KeyPose == keyPose)
					{
						material.color = Color.red;
					}
				}
			}
		}
	}
	void Setup()
	{
		// Add the keypoints into a list.
		foreach (MLHand hand in _hands)
		{
			_keypoints.AddRange(hand.Thumb.KeyPoints);
			_keypoints.AddRange(hand.Index.KeyPoints);
			_keypoints.AddRange(hand.Middle.KeyPoints);
			_keypoints.AddRange(hand.Ring.KeyPoints);
			_keypoints.AddRange(hand.Pinky.KeyPoints);
			_keypoints.AddRange(hand.Wrist.KeyPoints);
		}

		// Instantiate a prefab sphere for each keypoint
		for (int i = 0; i < _keypoints.Count; ++i)
		{
			GameObject newObject = Instantiate(prefab, _keypoints[i].Position,
				Quaternion.identity);
			_trackingPoints.Add(newObject);
		}
	}
}
