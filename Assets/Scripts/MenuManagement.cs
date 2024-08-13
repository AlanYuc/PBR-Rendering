using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManagement : MonoBehaviour
{
    private GameObject menu;
    private GameObject setting;
    private GameObject info;
    private GameObject help;

    // Start is called before the first frame update
    void Start()
    {
        //menu = transform.Find("Menu").gameObject;
        //setting = transform.Find("Setting").gameObject;
        //info = transform.Find("Info").gameObject;

        setting = transform.parent.Find("SettingPanel").gameObject;
        info = transform.parent.Find("InfoPanel").gameObject;
        help = transform.parent.Find("HelpPanel").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickMenu()
    {
        menu.SetActive(true);
    }

    public void OnClickInfo()
    {
        info.SetActive(true);
    }

    public void OnClickSetting()
    {
        setting.SetActive(true);
    }

    public void OnClickHelp()
    {
        help.SetActive(true);
    }
}
