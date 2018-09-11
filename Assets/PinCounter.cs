using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinCounter : MonoBehaviour
{
    public int pinCounter;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Pin")
        {
            pinCounter += 1;
        }
    }
}
