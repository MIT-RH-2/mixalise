using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoCircle : MonoBehaviour
{
 
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(
            Mathf.Cos(Time.time) * 5f,
            0f,
            Mathf.Sin(Time.time) * 5f
        );
    }
}
