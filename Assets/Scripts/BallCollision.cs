using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollision : MonoBehaviour
{
    public bool collidedWithLine = false;
    public bool collidedWithFloor = false;

    public bool isGrounded;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Line")
        {
            collidedWithLine = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Floor" && isGrounded == false)
        {
            collidedWithFloor = true;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "Floor")
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

}
