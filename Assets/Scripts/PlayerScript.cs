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
    public AudioSource ballRollSound;

    public float rotSpeed = 2f;
    public float alleyCamSmooth = 0.8f;

    public float mouseDirX;
    public float mouseDirY;

    public float speed = 200f;
    public bool crossedLine = false;
    public Vector3 vectorZero = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        ballRB = ball.GetComponent<Rigidbody>();
        ballCol = ball.GetComponent<Collider>();
        ballRollSound = ball.GetComponent<AudioSource>();

        transform.eulerAngles = new Vector3(0f, 180f, 0f);

    }


    void Update ()
    {
        crossedLine = ball.GetComponent<BallCollision>().collidedWithLine;

        mouseDirX = Input.GetAxis("Mouse X") * speed;
        mouseDirY = Input.GetAxis("Mouse Y") * speed;

        if (Input.touchCount > 0)
        {
            mouseDirX = Input.touches[0].deltaPosition.x;
            mouseDirY = Input.touches[0].deltaPosition.y;
        }

    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        if(ballRB.velocity != vectorZero)
        {
            ballRollSound.volume = ballRB.velocity.magnitude / 40f;
            ballRollSound.pitch = ballRB.velocity.magnitude / 60f + 1;
        }

        if (crossedLine == false)
        {
            //Smooth pivot rotation.
            rot = Quaternion.LookRotation(ballRB.velocity, Vector3.up);
            smoothRot = Quaternion.Slerp(this.transform.rotation, rot, rotSpeed * Time.deltaTime);
            transform.rotation = smoothRot;

            //Rotates Vector3 Y axis relative to the axis of this game object.
            mouseDirVector = Quaternion.AngleAxis(this.transform.eulerAngles.y, Vector3.up) * new Vector3(mouseDirX, 0f, mouseDirY);

            //Pivot point follow ball.
            transform.position = ball.transform.position;
        }

        else
        {
            //Pan to alleyCam position and rotation smoothly.
            this.transform.position = Vector3.SmoothDamp(this.transform.position, alleyCam.transform.position, ref vectorZero, alleyCamSmooth);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, alleyCam.transform.rotation, rotSpeed * Time.deltaTime);
        }
            //Add force relative to pivot rotation.
            ballRB.AddForce(mouseDirVector);
    }
}
