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
            Debug.Log("�˳�����");
            Application.Quit();
        }
    }

    public void OnClickStart()
    {
        SceneManager.LoadScene(1);
    }

    public void OnClickIntroduction()
    {
        //��ʾ�������
        Debug.Log("��ʾ�������");
    }

    public void OnClickQuit()
    {
        Debug.Log("�˳�����");
        Application.Quit();
    }
}
