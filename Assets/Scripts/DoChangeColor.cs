using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoChangeColor : MonoBehaviour
{
    public void GoYellow() {
        GoColor(Color.yellow);
    }

    public void GoGreen() {
        GoColor(Color.green);
    }

    public void GoRed() {
        GoColor(Color.red);
    }

    void GoColor(Color c) {
        gameObject.GetComponent<MeshRenderer>().material.color = c;
    }
}
