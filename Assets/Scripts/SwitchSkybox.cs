using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class SwitchSkybox : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    private Material skyboxBlue;
    private Material skyboxPink;
    private Material skyboxNight;

    private Transform blueTrans;
    private Transform pinkTrans;
    private Transform nightTrans;

    private Toggle blueToggle;
    private Toggle pinkToggle;
    private Toggle nightToggle;

    private bool isSkyboxChanged = false;

    //get tips panel
    private Transform tips;
    private Text contentMask;
    private Text content;
    private CanvasGroup canvasGroup;
    private string defaultText;
    private float targetAlpha;
    private float speed;

    void Start()
    {
        skyboxBlue = Resources.Load<Material>("Sky/Epic_BlueSunset/Epic_BlueSunset");
        skyboxPink = Resources.Load<Material>("Sky/Epic_GloriousPink/Epic_GloriousPink");
        skyboxNight = Resources.Load<Material>("Sky/Night MoonBurst/Night Moon Burst");

        if (skyboxBlue == null || skyboxNight == null || skyboxPink == null) 
        {
            Debug.Log("no material");
        }

        //Default skybox
        RenderSettings.skybox = skyboxBlue;
        DynamicGI.UpdateEnvironment();

        blueTrans = transform.Find("Blue");
        pinkTrans = transform.Find("Pink");
        nightTrans = transform.Find("Night");

        if(blueTrans ==null || pinkTrans==null || nightTrans == null)
        {
            Debug.Log("no skybox");
        }

        blueToggle = blueTrans.GetComponent<Toggle>();
        pinkToggle = pinkTrans.GetComponent<Toggle>();
        nightToggle = nightTrans.GetComponent<Toggle>();

        if (blueToggle == null || pinkToggle == null || nightToggle == null)
        {
            Debug.Log("no toggle");
        }

        tips = transform.Find("Tips");
        if(tips == null)
        {
            Debug.Log("no tips");
        }

        contentMask = tips.GetComponent<Text>();
        content = tips.transform.Find("Content").GetComponent<Text>();
        if(content ==null || contentMask == null)
        {
            Debug.Log("no content");
        }

        canvasGroup = tips.GetComponent<CanvasGroup>();
        if(canvasGroup == null)
        {
            Debug.Log("no canvasGroup");
        }

        defaultText = "Skybox:\n";
        targetAlpha = 0.0f;
        speed = 7.0f;
    }

    void Update()
    {
        if (isSkyboxChanged)
        {
            if (blueToggle.isOn)
            {
                RenderSettings.skybox = skyboxBlue;
            }
            else if (pinkToggle.isOn)
            {
                RenderSettings.skybox = skyboxPink;
            }
            else if (nightToggle.isOn)
            {
                RenderSettings.skybox = skyboxNight;
            }
            DynamicGI.UpdateEnvironment();

            isSkyboxChanged = false;
        }

        if(canvasGroup.alpha != targetAlpha)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, Time.deltaTime * speed);
            if(Mathf.Abs(canvasGroup.alpha - targetAlpha) < 0.005f)
            {
                canvasGroup.alpha = targetAlpha;
            }
        }

        if (blueTrans.GetComponent<IPointerEnter>().isPointerEnter)
        {
            content.text = defaultText + "Epic_BlueSunset_UI";
            contentMask.text = defaultText + "Epic_BlueSunset_UI";
        }
        else if (pinkTrans.GetComponent<IPointerEnter>().isPointerEnter)
        {
            content.text = defaultText + "Epic_GloriousPink_UI";
            contentMask.text = defaultText + "Epic_GloriousPink_UI";
        }
        else if (nightTrans.GetComponent<IPointerEnter>().isPointerEnter)
        {
            content.text = defaultText + "Night Moon Burst_UI";
            contentMask.text = defaultText + "Night Moon Burst_UI";
        }

        if(canvasGroup.alpha > 0)
        {
            tips.position = Input.mousePosition;
        }
    }

    public void OnValueChanged(bool isOn)
    {
        isSkyboxChanged = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetAlpha = 0.0f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetAlpha = 1.0f;
    }
}
