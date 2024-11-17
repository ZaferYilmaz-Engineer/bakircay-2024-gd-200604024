using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DraggableObject : MonoBehaviour
{ 
    public bool isBeingDragged;
    
    private Rigidbody rb;
    private float dragSpeed = 2.5f;
    private float yThreshold = 4f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void JumpToDragStartPosition()
    {
        isBeingDragged = true;
        rb.isKinematic = true;

        float endValue = rb.transform.position.y + 2f;
        endValue = Mathf.Clamp(endValue, 0, yThreshold);
        
        rb.DOMoveY(endValue, 0.2f);
    }

    public void Drag(Vector2 offset, GameArea.Boundaries boundaries)
    {
        rb.position += new Vector3(offset.x, 0, offset.y) * (Time.deltaTime * dragSpeed);
        
        var clampedXPosition = rb.position.x;
        clampedXPosition = Mathf.Clamp(clampedXPosition, boundaries.leftBoundary, boundaries.rightBoundary);
        
        var clampedZPosition = rb.position.z;
        clampedZPosition = Mathf.Clamp(clampedZPosition, boundaries.bottomBoundary, boundaries.topBoundary);

        rb.position = new Vector3(clampedXPosition, rb.position.y, clampedZPosition);
    }
    
    public void Drop()
    {
        isBeingDragged = false;
        rb.isKinematic = false;
        transform.DOKill();
    }

    public void LaunchToSpawnPosition()
    {
        Vector3 forceVector = new Vector3(0, 1, 1);
        float forceMagnitude = 10;
        forceVector *= forceMagnitude;
        rb.AddForce(forceVector, ForceMode.Impulse);
    }
}
