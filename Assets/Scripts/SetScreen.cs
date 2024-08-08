using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetScreen : MonoBehaviour
{

    public void OnWindow()
    {
        Screen.SetResolution(1920, 1080, false);
    }

    public void OnFullScreen()
    {
        Screen.SetResolution(1920, 1080, true);
    }
}
