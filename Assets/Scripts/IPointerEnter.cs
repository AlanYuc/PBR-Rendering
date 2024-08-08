using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IPointerEnter : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    public bool isPointerEnter;

    // Start is called before the first frame update
    void Start()
    {
        isPointerEnter = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerEnter=true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerEnter= false;
    }
}
