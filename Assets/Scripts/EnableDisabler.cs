using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisabler : MonoBehaviour
{
    public void DoEnable() {
        this.enabled = true;
    }

    public void DoDisable() {
        this.enabled = false;
    }
}
