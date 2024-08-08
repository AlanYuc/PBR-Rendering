using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShowHidePanel : MonoBehaviour
{
    private GameObject showPanel;
    private GameObject hidePanel;
    private GameObject parentPanel;

    private Toggle isShow;

    private float inGamePos = 0;
    private float outGamePos = 0;
    private float targetPos = 0;
    private int animSpeed = 6;
    private bool isAnimating = false;


    // Start is called before the first frame update
    void Start()
    {
        isShow = GetComponent<Toggle>();
        showPanel = transform.Find("Show").GameObject();    
        hidePanel = transform.Find("Hide").GameObject();
        parentPanel = transform.parent.GameObject();

        RectTransform rect = parentPanel.GetComponent<RectTransform>();
        if (rect != null)
        {
            inGamePos = rect.rect.width / 2;
            outGamePos = -inGamePos;
        }

        Debug.Log("inGamePos = " + inGamePos);
        Debug.Log("outGamePos = " + outGamePos);

    }

    // Update is called once per frame
    void Update()
    {
        if (isAnimating)
        {
            //transform. = Mathf.Lerp(transform.position.x, targetPos, Time.deltaTime);
            Vector3 tempPos = transform.parent.transform.position;
            //Debug.Log(tempPos);
            tempPos.x = Mathf.Lerp(tempPos.x, targetPos, Time.deltaTime * animSpeed);
            transform.parent.transform.position = tempPos;

            if (Mathf.Abs(tempPos.x - targetPos) < 0.005f)
            {
                isAnimating = false;
                tempPos.x = targetPos;
                transform.parent.transform.position = tempPos;
            }
        }
    }

    public void OnValueChanged(bool isOn)
    {
        showPanel.SetActive(isOn);
        hidePanel.SetActive(!isOn);
        isAnimating = true;

        if (!isOn)
        {
            targetPos = inGamePos;
        }
        else
        {
            targetPos = outGamePos;
        }
    }
}
