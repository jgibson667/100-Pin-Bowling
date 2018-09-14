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

    public AudioSource ballSound;

    public float smoothSpeed = 0.8f;
    public float smoothRotSpeed = 1.5f;

    public float mouseDirX;
    public float mouseDirY;

    public float speed = 200f;

    public bool collidedWithLine = false;
    public bool collidedWithFloor = false;
    public bool isGrounded;

    public Vector3 vectorZero = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        ballRB = ball.GetComponent<Rigidbody>();
        ballCol = ball.GetComponent<Collider>();
        ballSound = ball.GetComponent<AudioSource>();
    }


    void Update ()
    {
        collidedWithLine = ball.GetComponent<BallCollision>().collidedWithLine;
        collidedWithFloor = ball.GetComponent<BallCollision>().collidedWithFloor;
        isGrounded = ball.GetComponent<BallCollision>().isGrounded;

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
        if(ballRB.velocity != vectorZero && isGrounded == true)
        {
            ballSound.volume = ballRB.velocity.magnitude / 40f;
            ballSound.pitch = ballRB.velocity.magnitude / 60f + 1.15f;
        }

        if (ballRB.velocity != vectorZero && isGrounded == false)
        {
            ballSound.volume = 0f;
        }

        if (collidedWithLine == false)
        {
            if(ballRB.velocity.magnitude != 0)
            {
                //Smooth pivot rotation.
                transform.rotation = smoothRot;
                rot = Quaternion.LookRotation(ballRB.velocity, Vector3.up);
                smoothRot = Quaternion.Lerp(this.transform.rotation, rot, smoothRotSpeed * Time.deltaTime);

                //Pivot point follow ball.
                transform.position = ball.transform.position;
            }

            //Rotates Vector3 Y axis relative to the axis of this game object.
            mouseDirVector = Quaternion.AngleAxis(this.transform.eulerAngles.y, Vector3.up) * new Vector3(mouseDirX, 0f, mouseDirY);            
        }

        else
        {
            //Pan to alleyCam position and rotation smoothly.
            this.transform.position = Vector3.SmoothDamp(this.transform.position, alleyCam.transform.position, ref vectorZero, smoothSpeed);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, alleyCam.transform.rotation, smoothRotSpeed * Time.deltaTime);
        }
            //Add force relative to pivot rotation.
            ballRB.AddForce(mouseDirVector);
    }
}
