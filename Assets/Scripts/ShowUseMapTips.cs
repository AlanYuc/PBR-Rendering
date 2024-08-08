using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowUseMapTips : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler
{
    private Transform tips;
    private Text contentMask;
    private Text content;
    private CanvasGroup canvasGroup;
    private string defaultText;
    private float targetAlpha;
    private float speed;


    // Start is called before the first frame update
    void Start()
    {
        tips = transform.Find("Tips");
        if (tips == null)
        {
            Debug.Log("no tips");
        }

        contentMask = tips.GetComponent<Text>();
        content = tips.transform.Find("Content").GetComponent<Text>();
        if (content == null || contentMask == null)
        {
            Debug.Log("no content");
        }

        canvasGroup = tips.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.Log("no canvasGroup");
        }

        defaultText = "\nUse MetallicMap";
        contentMask.text = defaultText;
        content.text = defaultText;
        targetAlpha = 0.0f;
        speed = 7.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (canvasGroup.alpha != targetAlpha)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, Time.deltaTime * speed);
            if (Mathf.Abs(canvasGroup.alpha - targetAlpha) < 0.005f)
            {
                canvasGroup.alpha = targetAlpha;
            }
        }

        if (canvasGroup.alpha > 0)
        {
            tips.position = Input.mousePosition;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        targetAlpha = 1.0f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetAlpha = 0.0f;
    }

}
