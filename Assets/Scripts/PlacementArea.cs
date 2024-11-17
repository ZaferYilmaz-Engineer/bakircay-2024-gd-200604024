using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlacementArea : MonoBehaviour
{
    private DraggableObject currentObject;
    
    private void OnTriggerEnter(Collider other)
    {
        if (currentObject)
        {
            if (other.TryGetComponent(out DraggableObject objectToLaunch))
            {
                HandleMultipleObject(objectToLaunch);
            }
            
            return;
        }
        
        if (!other.TryGetComponent(out DraggableObject draggableObject) || draggableObject.isBeingDragged)
        {
            return;
        }

        currentObject = draggableObject;
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

    private void HandleMultipleObject(DraggableObject objectToLaunch)
    {
        WarningUI.Instance.ShowWarning();
        objectToLaunch.LaunchToSpawnPosition();
    }
}
