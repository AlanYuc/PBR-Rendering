using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinModel : MonoBehaviour
{
    private bool isClick = false;
    private Vector3 newPos;
    private Vector3 oldPos;
    private float length;

    private Quaternion defaultQ;

    private Quaternion newQuaternion;
    private Quaternion oldQuaternion;
    private float spinSpeed = 30;
    private bool isReset = false;

    private void OnMouseUp()
    {
        isClick = false;
        //isReset = true;
    }

    private void OnMouseDown()
    {
        isClick = true;
    }

    private void Start()
    {
        length = 0;
        defaultQ = transform.rotation;
    }

    private void Update()
    {
        if (isClick)
        {
            newPos = Input.mousePosition;

            Vector3 offset = newPos - oldPos;
            if(Mathf.Abs(offset.x) > Mathf.Abs(offset.y) && Mathf.Abs(offset.x) > length)
            {
                //Vector3 targetRotation = new Vector3(0, -offset.x, 0);
                //oldQuaternion = transform.rotation;
                //newQuaternion = Quaternion.Euler(targetRotation);
                //transform.rotation = Quaternion.Lerp(oldQuaternion, newQuaternion, spinSpeed * Time.deltaTime);
                
                
                transform.Rotate(Vector3.up, -offset.x);
            }

            oldPos = newPos;
        }

        //if (isReset)
        //{
        //    Debug.Log(isReset);
        //    transform.rotation = Quaternion.Lerp(transform.rotation, defaultQ, spinSpeed * Time.deltaTime);
        //    //Mathf.Abs(transform.eulerAngles.y)
        //    if (Quaternion.Angle(transform.rotation, defaultQ) < 0.1f)
        //    {
        //        transform.rotation = defaultQ;
        //        isReset = false;
        //    }
        //}
    }
}
