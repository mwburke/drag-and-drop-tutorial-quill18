using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        // I probably won't need any of this swapping between areas in my game
        // Just targets of things and if they are valid targets
        
        if (eventData.pointerDrag == null)
        {
            return;
        }

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();

        if (d != null)
        {
            d.placeholderParent = this.transform;
        }
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            return;
        }   

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null && d.placeholderParent == this.transform)
        {
            d.placeholderParent = d.parentToReturnTo;
        }
    }


    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name + " was dropped on to " + gameObject.name);

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();

        if (d != null)
        {
            d.parentToReturnTo = this.transform;
        }
    }

}
