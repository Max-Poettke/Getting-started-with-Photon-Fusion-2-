using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class HoverInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public List<string> Descriptions = new List<string>();
    private bool isHovering = false;

    public void OnPointerEnter(PointerEventData eventData){
        isHovering = true;
        foreach(var description in Descriptions){
            HoverInfoUI.Instance.DisplayInfo(description);
        }
    }

    public void OnPointerExit(PointerEventData eventData){
        isHovering = false;
        HoverInfoUI.Instance.ClearInfo();
    }

    public void OnDestroy(){
        if(isHovering){
            HoverInfoUI.Instance.ClearInfo();
        }
    }
}
