using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDragController : MonoBehaviour
{
    [SerializeField] private LayerMask draggableLayer;
    [SerializeField] private LayerMask groundLayer;
    
    private DraggableObject currentObject;
    private GameArea.Boundaries boundaries;
    
    private void Start()
    {
        TouchManager.Instance.OnTouchBegin += OnTouchBegin;
        TouchManager.Instance.OnTouchHold += OnTouchHold;
        TouchManager.Instance.OnTouchEnd += OnTouchEnd;
        boundaries = GameArea.Instance.GetBoundaries();
    }

    private void OnTouchBegin(TouchManager.TouchData touchData)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchData.position);

        if (!Physics.Raycast(ray, out RaycastHit hit, 999f, draggableLayer))
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
    
    private void OnTouchHold(TouchManager.TouchData touchData)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchData.position);

        if (!Physics.Raycast(ray, out RaycastHit hit, 999f, groundLayer))
        {
            return;
        }
        
        Debug.DrawRay(hit.point, Vector3.up, Color.red);
        currentObject?.Drag(hit.point, boundaries);
    }

    private void OnTouchEnd(TouchManager.TouchData touchData)
    {
        currentObject?.Drop();
        currentObject = null;
    }
    
    private void OnDestroy()
    {
        TouchManager.Instance.OnTouchBegin -= OnTouchBegin;
        TouchManager.Instance.OnTouchMove -= OnTouchHold;
        TouchManager.Instance.OnTouchEnd -= OnTouchEnd;
    }
}
