using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDragController : MonoBehaviour
{
    [SerializeField] private LayerMask draggableLayer;
    
    private DraggableObject currentObject;
    private GameArea.Boundaries boundaries;
    
    private void Start()
    {
        TouchManager.Instance.OnTouchBegin += OnTouchBegin;
        TouchManager.Instance.OnTouchMove += OnTouchMove;
        TouchManager.Instance.OnTouchEnd += OnTouchEnd;
        boundaries = GameArea.Instance.GetBoundaries();
        
        Debug.Log($"left: {boundaries.leftBoundary}");
        Debug.Log($"right: {boundaries.rightBoundary}");
        Debug.Log($"bottom: {boundaries.bottomBoundary}");
        Debug.Log($"top: {boundaries.topBoundary}");
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
    
    private void OnTouchMove(TouchManager.TouchData touchData)
    {
        currentObject?.Drag(touchData.deltaPosition, boundaries);
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
