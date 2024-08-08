using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class testUnityState : MonoBehaviour
{
    private float fps;
    private int drawCall;
    private float frameTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //fps = Time.realtimeSinceStartup;
        //frameTime = UnityStats.frameTime;
        fps =  1 / frameTime;
        //fps = Mathf.Clamp01(fps);
        //drawCall = UnityStats.drawCalls;
        //Debug.Log(fps);
        //Debug.Log(drawCall);
    }
}
