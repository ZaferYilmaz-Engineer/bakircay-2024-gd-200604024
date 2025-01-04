using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlacementArea : MonoBehaviour
{
    public static Action<bool> OnAnyObjectsPaired;

    [SerializeField] private Transform pairSuccessAnimationTargetTransform;
    
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
        currentObject.SetKinematicState(true);
        
        var targetPosition = transform.position + Vector3.up * 0.5f;
        currentObject.transform.DOMove(targetPosition, 1f);
        currentObject.transform.DORotate(Vector3.zero, 1f);
    }
    
    private void HandleMultipleObject(DraggableObject secondObject)
    {
        if (secondObject.isBeingDragged)
        {
            return;
        }
        
        if (currentObject.DraggableObjectSO == secondObject.DraggableObjectSO)
        {
            TouchManager.Instance.isTouchEnabled = false;
            DoPairSuccessSequence(secondObject);
        }
        else
        {
            WarningUI.Instance.ShowWarning();
            secondObject.LaunchToSpawnPosition();
            OnAnyObjectsPaired?.Invoke(false);
        }
    }

    private void DoPairSuccessSequence(DraggableObject secondObject)
    {
        currentObject.SetPhysicsState(false);
        secondObject.SetPhysicsState(false);
        
        Sequence generalSequence = DOTween.Sequence();
        Sequence firstSuccessAnimationSequence = DOTween.Sequence();
        Sequence finalSequence = DOTween.Sequence();
        
        Tween currentObjectPositionTween = DoMoveTween(currentObject.transform);
        Tween currentObjectRotationTween = DoRotateTween(currentObject.transform);
        
        Tween secondObjectPositionTween = DoMoveTween(secondObject.transform);
        Tween secondObjectRotationTween = DoRotateTween(secondObject.transform);

        firstSuccessAnimationSequence.Append(currentObjectPositionTween);
        firstSuccessAnimationSequence.Join(currentObjectRotationTween);
        firstSuccessAnimationSequence.Join(secondObjectPositionTween);
        firstSuccessAnimationSequence.Join(secondObjectRotationTween);
        
        generalSequence.Append(firstSuccessAnimationSequence);
        firstSuccessAnimationSequence.onComplete += () =>
        {
            secondObject.DestroySelf();
            OnAnyObjectsPaired?.Invoke(true);
        };
        
        Tween finalScaleTween = currentObject.transform.DOPunchScale(Vector3.one * 0.2f, 0.5f);
        finalSequence.Append(finalScaleTween);
        
        generalSequence.Append(finalSequence);

        generalSequence.onComplete += () =>
        {
            currentObject.DestroySelf();
            TouchManager.Instance.isTouchEnabled = true;
        };
    }

    private Tween DoMoveTween(Transform transformToTween)
    {
        Tween tween = transformToTween.DOMove(pairSuccessAnimationTargetTransform.position, 1f);
        return tween;
    }

    private Tween DoRotateTween(Transform transformToTween)
    {
        var tween = transformToTween.DORotate(Vector3.zero, 1f);
        return tween;
    }
}
