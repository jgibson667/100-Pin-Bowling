using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollision : MonoBehaviour
{
    public bool collidedWithLine = false;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Line")
        {
            collidedWithLine = true;
        }
    }
}
