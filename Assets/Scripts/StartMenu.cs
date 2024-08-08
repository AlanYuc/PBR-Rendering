using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("退出程序");
            Application.Quit();
        }
    }

    public void OnClickStart()
    {
        SceneManager.LoadScene(1);
    }

    public void OnClickIntroduction()
    {
        //显示介绍面板
        Debug.Log("显示介绍面板");
    }

    public void OnClickQuit()
    {
        Debug.Log("退出程序");
        Application.Quit();
    }
}
