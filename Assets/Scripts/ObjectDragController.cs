using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDragController : MonoBehaviour
{
    private DraggableObject currentObject;
    
    private void Start()
    {
        TouchManager.Instance.OnTouchBegin += OnTouchBegin;
        TouchManager.Instance.OnTouchMove += OnTouchMove;
        TouchManager.Instance.OnTouchEnd += OnTouchEnd;
    }

    private void OnTouchBegin(TouchManager.TouchData touchData)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchData.position);

        if (!Physics.Raycast(ray, out RaycastHit hit, 999f))
        {
            return;
        }

        if (!hit.transform.TryGetComponent(out DraggableObject draggableObject))
        {
            return;
        }

        currentObject = draggableObject;
        currentObject.JumpToDragStartPosition();
    }
    
    private void OnTouchMove(TouchManager.TouchData touchData)
    {
        currentObject?.Drag(touchData.deltaPosition);
    }

    private void OnTouchEnd(TouchManager.TouchData touchData)
    {
        currentObject?.Drop();
        currentObject = null;
    }
    
    private void OnDestroy()
    {
        TouchManager.Instance.OnTouchBegin -= OnTouchBegin;
        TouchManager.Instance.OnTouchMove -= OnTouchMove;
        TouchManager.Instance.OnTouchEnd -= OnTouchEnd;
    }
}
