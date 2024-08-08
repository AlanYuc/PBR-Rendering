using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchControlPanel : MonoBehaviour
{
    public GameObject ShaderProperties;
    public GameObject ShaderProperties2;
    public GameObject ShaderProperties3;

    // Start is called before the first frame update
    void Start()
    {
        ShaderProperties.SetActive(true); 
        ShaderProperties2.SetActive(false);
        ShaderProperties3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnValueChanged(int value)
    {
        switch (value) { 
            case 0:
                ShaderProperties.SetActive(true);
                ShaderProperties2.SetActive(false);
                ShaderProperties3.SetActive(false);
                break;
            case 1:
                ShaderProperties.SetActive(false);
                ShaderProperties2.SetActive(true);
                ShaderProperties3.SetActive(false);
                break;
            case 2:
                ShaderProperties.SetActive(false);
                ShaderProperties2.SetActive(false);
                ShaderProperties3.SetActive(true);
                break;
        }
    }
}
