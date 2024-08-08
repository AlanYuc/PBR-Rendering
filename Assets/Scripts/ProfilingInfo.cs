using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ProfilingInfo : MonoBehaviour
{
    private float fps;
    private float frameTime;
    private int drawCalls;
    private float renderTime;
    private int triangles;
    private int vertices;

    private Text info;

    private float timer;
    private float updateInterval;
    private bool isUpdate;

    // Start is called before the first frame update
    void Start()
    {
        info = transform.Find("Bg/Text").GetComponent<Text>();
        if(info == null)
        {
            Debug.Log("no info");
        }

        fps = 0;
        frameTime = 0;
        drawCalls = 0;
        triangles = 0;
        vertices = 0;
        renderTime = 0;

        timer = 0;
        updateInterval = 0.2f;
        isUpdate = true;
    }

    // Update is called once per frame
    void Update()
    {
        #if UNITY_EDITOR
        frameTime = UnityStats.frameTime;
        fps = 1 / frameTime;
        drawCalls = UnityStats.drawCalls;
        renderTime = UnityStats.renderTime;
        triangles = UnityStats.triangles;
        vertices = UnityStats.vertices;
        #endif

        if (isUpdate)
        {
            //string.Format("FPS:{0:0.00}\n", fps)
            info.text =
                 "FPS:" + fps + "\n" +
                "Frame Time:" + frameTime + "\n" +
                "Render Time:" + renderTime + "\n" +
                "Draw Calls:" + drawCalls + "\n" +
                "Triangles:" + triangles + "\n" +
                "Vertices:" + vertices;

            isUpdate = false;
        }

        timer += Time.deltaTime;
        if (timer > updateInterval)
        {
            timer = 0;
            isUpdate = true;
        }
    }

    public void OnCloseButtonClick()
    {
        transform.gameObject.SetActive(false);
    }
}
