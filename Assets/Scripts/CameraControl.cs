using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private float speed;
    private float speedR;
    private float updownSpeed;
    private string verticalAsixName;
    private string horizontalAsixName; 
    private float vertical;

    private float maxAngle = 90f;
    private float minAngle = -90f;

    private float lerpSpeed;
    private float targetRotationX = 0f;
    private float targetRotationY = 0f;

    private Vector3 defaultPos;
    private Quaternion defaultRot;

    // Start is called before the first frame update
    void Start()
    {
        speed = 5;
        speedR = 5;
        lerpSpeed = 5;
        updownSpeed = 2;
        verticalAsixName = "Vertical";
        horizontalAsixName = "Horizontal";

        defaultPos = transform.position;
        defaultRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (Camera.main.fieldOfView <= 100)
            {
                Camera.main.fieldOfView += 2;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (Camera.main.fieldOfView > 2)
            {
                Camera.main.fieldOfView -= 2;
            }
        }

        float horizontalMovement = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float verticalMovement = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        transform.Translate(horizontalMovement, 0, verticalMovement);

        if (Input.GetMouseButton(1))
        {
            float mouseX = -Input.GetAxis("Mouse Y");
            float mouseY = Input.GetAxis("Mouse X");

            targetRotationX += mouseX * speedR;
            targetRotationY += mouseY * speedR;

            targetRotationX = Mathf.Clamp(targetRotationX, minAngle, maxAngle);

            Quaternion targetRotation = Quaternion.Euler(0, targetRotationY, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, lerpSpeed * Time.deltaTime);

            //transform.Rotate(-mouseY, mouseX, 0);
        }

        if (Input.GetKey(KeyCode.Z))
        {
            transform.Translate(new Vector3(0, -updownSpeed * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.X))
        {
            transform.Translate(new Vector3(0, updownSpeed * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.R))
        {
            transform.position = defaultPos;
            transform.rotation = defaultRot;
        }

    }
}
