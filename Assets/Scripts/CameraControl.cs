using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private float speed;
    private string verticalAsixName;
    private float vertical;

    // Start is called before the first frame update
    void Start()
    {
        speed = 5;
        verticalAsixName = "Vertical";
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

        vertical = Input.GetAxis(verticalAsixName);
        transform.Translate(new Vector3(0, vertical, 0) * speed * Time.deltaTime);
    }
}
