using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinDetection : MonoBehaviour
{
    public bool hasFallen = false;

    // Use this for initialization
    void FixedUpdate()
    {
        if(this.transform.eulerAngles.x > 35f || this.transform.eulerAngles.x < -35f ||
           this.transform.eulerAngles.y > 35f || this.transform.eulerAngles.y < -35f)
        {
            Fallen();
        }

    }

    void Fallen()
    {
        hasFallen = true;
    }
}