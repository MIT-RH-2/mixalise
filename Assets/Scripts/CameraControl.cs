using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public List<Camera> cameras;
    private int currentCamera = 0;

    public void Update() {

        for (int i = 0; i < cameras.Count; i++) {
            int keyNo = i + 1 % 10;
            if (Input.GetKeyDown($"{keyNo}")) {
                currentCamera = i;
                break;
            }
        }

        cameras.ForEach(x => x.gameObject.SetActive(false));
        cameras[currentCamera].gameObject.SetActive(true);
    }
}
