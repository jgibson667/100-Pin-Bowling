using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameObject ball;

    Rigidbody ballRB;
    Collider ballCol;
    Quaternion rot;
    Quaternion smoothRot;
    Vector3 mouseDirVector;
    GameObject alleyCam;

    float smoothSpeed = 2f;
    float smoothRotSpeed = 0.8f;

    float mouseDirX;
    float mouseDirY;

    public float speed = 50f;
    public float touchSpeed = 20f;

    bool collidedWithLine = false;
    bool collidedWithFloor = false;
    bool isGrounded;

    Vector3 vectorZero = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        ballRB = ball.GetComponent<Rigidbody>();
        ballCol = ball.GetComponent<Collider>();
        this.transform.rotation = Quaternion.identity;
    }


    void Update ()
    {
        collidedWithLine = ball.GetComponent<BallCollision>().collidedWithLine;
        collidedWithFloor = ball.GetComponent<BallCollision>().collidedWithFloor;
        isGrounded = ball.GetComponent<BallCollision>().isGrounded;

        if (collidedWithLine == false)
        {
            mouseDirX = Input.GetAxis("Mouse X") / Screen.width * 2000f * speed;
            mouseDirY = Input.GetAxis("Mouse Y") / Screen.height * 1000f * speed;

            if (Input.touchCount > 0)
            {

                mouseDirX = Input.GetTouch(0).deltaPosition.x / Screen.width * 2000f * touchSpeed;
                mouseDirY = Input.GetTouch(0).deltaPosition.y / Screen.height * 1000f * touchSpeed;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        if (collidedWithLine == false)
        {
            //Pivot point follow ball.
            transform.position = ball.transform.position;

            Debug.Log(ballRB.velocity);

            //Smooth pivot rotation.
            transform.rotation = smoothRot;
            if (ballRB.velocity != Vector3.zero)
            {
                rot = Quaternion.LookRotation(new Vector3(ballRB.velocity.x,0f,ballRB.velocity.z), Vector3.up);
            }

            smoothRot = Quaternion.Lerp(this.transform.rotation, rot, smoothRotSpeed * Time.deltaTime);

            //Rotates Vector3 Y axis relative to the axis of this game object.
            mouseDirVector = Quaternion.AngleAxis(this.transform.eulerAngles.y, Vector3.up) * new Vector3(mouseDirX, 0f, mouseDirY);

            //Add force relative to pivot rotation.
            ballRB.AddForce(mouseDirVector);
        }

        else
        {
            //Pan to alleyCam position and rotation smoothly.
            this.transform.position = Vector3.SmoothDamp(this.transform.position, alleyCam.transform.position, ref vectorZero, smoothSpeed);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, alleyCam.transform.rotation, smoothRotSpeed * Time.deltaTime);
        }
    }
}
