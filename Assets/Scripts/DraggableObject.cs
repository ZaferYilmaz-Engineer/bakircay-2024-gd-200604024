using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DraggableObject : MonoBehaviour
{ 
    private Rigidbody rb;
    private float dragSpeed = 2.5f;
    private float yThreshold = 4f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void JumpToDragStartPosition()
    {
        rb.isKinematic = true;
        rb.useGravity = false;

        float endValue = rb.transform.position.y + 2f;
        endValue = Mathf.Clamp(endValue, 0, yThreshold);
        
        rb.DOMoveY(endValue, 0.2f);
    }

    public void Drag(Vector2 offset)
    {
        rb.position += new Vector3(offset.x, 0, offset.y) * (Time.deltaTime * dragSpeed);
    }
    
    public void Drop()
    {
        rb.isKinematic = false;
        transform.DOKill();
        rb.useGravity = true;
    }
}
