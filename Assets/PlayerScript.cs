using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameObject ball;
    public Rigidbody ballRB;
    public Collider ballCol;
    public Quaternion rot;
    public Quaternion smoothRot;
    public Vector3 mouseDirVector;
    public GameObject alleyCam;

    public float rotSpeed = 2f;

    public float mouseDirX;
    public float mouseDirY;
    public float speed = 200f;
    public bool crossedLine = false;

    public Vector3 ballVelocity;

    // Use this for initialization
    void Start()
    {
        ballRB = ball.GetComponent<Rigidbody>();
        ballCol = ball.GetComponent<Collider>();
        ballVelocity = ballRB.velocity;

        transform.eulerAngles = new Vector3(0f, 180f, 0f);

    }


    void Update ()
    {
        if(crossedLine == false)
        {
            mouseDirX = Input.GetAxis("Mouse X") * speed;
            mouseDirY = Input.GetAxis("Mouse Y") * speed;

            //Smooth pivot rotation.
            rot = Quaternion.LookRotation(ballRB.velocity, Vector3.up);
            smoothRot = Quaternion.Slerp(transform.rotation, rot, rotSpeed * Time.deltaTime);
            transform.rotation = smoothRot;

            //Rotates Vector3 Y axis relative to the axis of this game object.
            mouseDirVector = Quaternion.AngleAxis(this.transform.eulerAngles.y, Vector3.up) * new Vector3(mouseDirX, 0f, mouseDirY);

            //Pivot point follow ball.
            transform.position = ball.transform.position;
        }
        else
        {
            this.transform.position = Vector3.SmoothDamp(this.transform.position, alleyCam.transform.position, ref ballVelocity, 0.8f);
        }
        crossedLine = ball.GetComponent<BallCollision>().collidedWithLine;
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        //Add force relative to pivot rotation.
        ballRB.AddForce(mouseDirVector);
    }
}
