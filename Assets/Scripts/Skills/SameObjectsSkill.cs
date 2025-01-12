using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SameObjectsSkill : BaseSkill
{
    private struct OriginalObjectData
    {
        public DraggableObjectSO originalDraggableObjectSO;
        public Vector3 lastPosition;
    }

    [SerializeField] private DraggableObjectSO[] draggableObjectSOArray;
    
    private List<OriginalObjectData> originalObjectDataList = new();
    
    protected override void HandleSkill()
    {
        StartCoroutine(HandleSkill());
        IEnumerator HandleSkill()
        {
            var activeDraggableObjectList = LevelManager.Instance.activeDraggableObjectList;
            originalObjectDataList.Clear();
            
            for (int i = activeDraggableObjectList.Count- 1; i >= 0; i--)
            {
                var activeDraggableObject = activeDraggableObjectList[i];
                
                originalObjectDataList.Add(new OriginalObjectData
                {
                    originalDraggableObjectSO = activeDraggableObject.DraggableObjectSO,
                    lastPosition = activeDraggableObject.transform.position
                });
                
                activeDraggableObject.DestroySelf();
            }
            
            yield return new WaitForEndOfFrame();

            var randomIndex = Random.Range(0, draggableObjectSOArray.Length);
            var selectedPrefab = draggableObjectSOArray[randomIndex].prefab;
            
            foreach (var originalObjectData in originalObjectDataList)
            {
                var spawnedDraggableObject = LevelManager.Instance.SpawnObjectAtPosition(selectedPrefab, 
                    originalObjectData.lastPosition + Vector3.up);

                spawnedDraggableObject.originalDraggableObjectSO = originalObjectData.originalDraggableObjectSO;
            }
        }
    }

    protected override void DeactivateSkill()
    {
        base.DeactivateSkill();

        if (LevelManager.Instance.isObjectsSpawning)
        {
            return;
        }
        
        StartCoroutine(HandleSkill());
        IEnumerator HandleSkill()
        {
            List<DraggableObject> activeDraggableObjectList = LevelManager.Instance.activeDraggableObjectList;
            originalObjectDataList.Clear();
            
            for (int i = activeDraggableObjectList.Count - 1; i >= 0; i--)
            {
                var activeDraggableObject = activeDraggableObjectList[i];
                
                originalObjectDataList.Add(new OriginalObjectData
                {
                    originalDraggableObjectSO = activeDraggableObject.originalDraggableObjectSO,
                    lastPosition = activeDraggableObject.transform.position
                });
                
                activeDraggableObject.DestroySelf();
            }
            
            yield return new WaitForEndOfFrame();
            
            foreach (var originalObjectData in originalObjectDataList)
            {
                LevelManager.Instance.SpawnObjectAtPosition(originalObjectData.originalDraggableObjectSO.prefab, 
                    originalObjectData.lastPosition + Vector3.up);
            }
        }
    }
}
