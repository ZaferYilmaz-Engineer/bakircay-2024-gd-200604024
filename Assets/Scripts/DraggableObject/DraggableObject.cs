using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    public static event Action<DraggableObject> OnBeingDestroyed;
    
    public DraggableObjectSO DraggableObjectSO => draggableObjectSO;
    public bool isBeingDragged;
    public DraggableObjectSO originalDraggableObjectSO;

    [SerializeField] private DraggableObjectSO draggableObjectSO;
    
    private Rigidbody rb;
    private Collider collider;
    private float dragSpeed = 1.2f;
    private float yThreshold = 4f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }
    
    public void JumpToDragStartPosition()
    {
        isBeingDragged = true;
        rb.isKinematic = true;

        float endValue = rb.transform.position.y + 2f;
        endValue = Mathf.Clamp(endValue, 0, yThreshold);
        
        rb.DOMoveY(endValue, 0.2f);
    }

    public void Drag(Vector3 mouseWorldPosition, GameArea.Boundaries boundaries)
    {
        rb.position = new Vector3(mouseWorldPosition.x, rb.position.y, mouseWorldPosition.z);
        
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
        rb.velocity = Vector3.zero;
    }

    public void LaunchToSpawnPosition()
    {
        Vector3 forceVector = new Vector3(0, 1, 1);
        float forceMagnitude = 10;
        forceVector *= forceMagnitude;
        rb.AddForce(forceVector, ForceMode.Impulse);
    }

    public void SetPhysicsState(bool isActive)
    {
        rb.isKinematic = !isActive;
        collider.enabled = isActive;
    }
    
    public void SetKinematicState(bool isKinematic)
    {
        rb.isKinematic = isKinematic;
    }

    public void DestroySelf()
    {
        OnBeingDestroyed?.Invoke(this);
        Destroy(gameObject);
    }
}
