using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    Material skybox;
    // Start is called before the first frame update
    void Start()
    {
        skybox = Resources.Load<Material>("Sky/Epic_BlueSunset/Epic_BlueSunset");
        DynamicGI.UpdateEnvironment();
        if (skybox != null)
        {
            //Debug.Log(skybox);
        }
        else
        {
            Debug.Log("no material");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(RenderSettings.skybox == null)
            {
                RenderSettings.skybox = skybox;
                Debug.Log(RenderSettings.skybox.name);
            }
            else
            {
                Material newSkybox = Resources.Load<Material>("Sky/Epic_GloriousPink/Epic_GloriousPink");
                RenderSettings.skybox = newSkybox;
                Debug.Log(RenderSettings.skybox.name);
            }
            DynamicGI.UpdateEnvironment();
        }
    }
}
