using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private Vector3 offset;
    public Transform parentToReturnTo = null;
    public Transform placeholderParent = null;

    GameObject placeholder = null;
    

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Debug.Log("OnBeginDrag");
        offset = this.transform.position - new Vector3(eventData.position.x, eventData.position.y, this.transform.position.z);

        placeholder = new GameObject();
        placeholder.transform.SetParent(this.transform.parent);
        

        LayoutElement le = placeholder.AddComponent<LayoutElement>();
        le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth * 1f / 0.95f;
        le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        le.flexibleWidth = this.GetComponent<LayoutElement>().flexibleWidth = 0;
        le.flexibleHeight = this.GetComponent<LayoutElement>().flexibleHeight = 0;

        placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        parentToReturnTo = this.transform.parent;
        placeholderParent = parentToReturnTo;
        this.transform.SetParent(this.transform.parent.parent);

        GetComponent<CanvasGroup>().blocksRaycasts = false;

        DropZone[] zones = GameObject.FindObjectsOfType<DropZone>();
        // Could find each valid zone and then make them glow
        // Then turn this off OnEndDrag. Just a nice UI thing
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log("OnDrag");

        this.transform.position = new Vector3(eventData.position.x, eventData.position.y, this.transform.position.z) + offset;

        if (placeholder.transform.parent != placeholderParent)
        {
            placeholder.transform.SetParent(placeholderParent);
        }

        int newSiblingIndex = placeholderParent.childCount;

        for (int i = 0; i < placeholderParent.childCount; i++)
        {
            if(this.transform.position.x < placeholderParent.GetChild(i).position.x)
            {
                newSiblingIndex = i;

                if (placeholder.transform.GetSiblingIndex() < newSiblingIndex)
                {
                    newSiblingIndex--;
                }
                
                break;
            }
        }

        placeholder.transform.SetSiblingIndex(newSiblingIndex);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Debug.Log("OnEndDrag");
        this.transform.SetParent(parentToReturnTo);

        GetComponent<CanvasGroup>().blocksRaycasts = true;

        // This raycasts through everything and finds out what's below you and filter to acceptable options
        // May be a good alternative to blocking and unblocking raycasts for objects
        // EventSystem.current.RaycastAll(eventData, );

        this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
        Destroy(placeholder);
    }   
}
