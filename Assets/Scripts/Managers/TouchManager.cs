using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public struct TouchData
    {
        public Vector2 position;
        public Vector2 deltaPosition;
    }
    
    public static TouchManager Instance { get; private set; }

    public bool isTouchEnabled;
    
    public Action<TouchData> OnTouchBegin;
    public Action<TouchData> OnTouchMove;
    public Action<TouchData> OnTouchEnd;
    public Action<TouchData> OnTouchHold;

    private Vector3 oldPosition;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        isTouchEnabled = true;
    }

    private void Update()
    {
        if (!isTouchEnabled)
        {
            return;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            HandleTouchBegin();
        }
        
        if (Input.GetMouseButton(0))
        {
            HandleTouchMove();
            HandleTouchHold();
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            HandleTouchEnd();
        }
    }

    private void HandleTouchBegin()
    {
        var touchData = new TouchData
        {
            position = Input.mousePosition
        };
        
        OnTouchBegin?.Invoke(touchData);
    }
    
    private void HandleTouchMove()
    {
        Vector3 deltaPosition = Vector3.zero;
        
        if (oldPosition != Vector3.zero)
        {
            deltaPosition = Input.mousePosition - oldPosition;
        }
        
        oldPosition = Input.mousePosition;
        
        var touchData = new TouchData
        {
            position = Input.mousePosition,
            deltaPosition =  deltaPosition
        };
        
        OnTouchMove?.Invoke(touchData);
    }

    private void HandleTouchHold()
    {
        if (Input.GetMouseButton(0))
        {
            var touchData = new TouchData
            {
                position = Input.mousePosition
            };
            
            OnTouchHold?.Invoke(touchData);
        }
    }
    
    private void HandleTouchEnd()
    {
        oldPosition = Vector3.zero;
        
        var touchData = new TouchData
        {
            position = Input.mousePosition
        };
        
        OnTouchEnd?.Invoke(touchData);
    }
}
