using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlacementArea : MonoBehaviour
{
    public static Action OnAnyObjectsPaired; 
        
    private DraggableObject currentObject;
    
    private void OnTriggerEnter(Collider collidedObject)
    {
        if (currentObject)
        {
            if (collidedObject.TryGetComponent(out DraggableObject secondObject))
            {
                HandleMultipleObject(secondObject);
            }
            
            return;
        }
        
        HandleFirstObject(collidedObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentObject == null)
        {
            return;
        }

        if (other.TryGetComponent(out DraggableObject objectToLeave) && objectToLeave == currentObject)
        {
            currentObject = null;
        }
    }

    private void HandleFirstObject(Collider collidedObject)
    {
        if (!collidedObject.TryGetComponent(out DraggableObject draggableObject) || draggableObject.isBeingDragged)
        {
            return;
        }

        currentObject = draggableObject;
    }
    
    private void HandleMultipleObject(DraggableObject secondObject)
    {
        if (secondObject.isBeingDragged)
        {
            return;
        }
        
        if (currentObject.DraggableObjectSO == secondObject.DraggableObjectSO)
        {
            currentObject.DestroySelf();
            secondObject.DestroySelf();
            OnAnyObjectsPaired?.Invoke();
        }
        else
        {
            WarningUI.Instance.ShowWarning();
            secondObject.LaunchToSpawnPosition();   
        }
    }
}
