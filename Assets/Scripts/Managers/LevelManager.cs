using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private DraggableObjectSO[] draggableObjectArray;
    [SerializeField] [Range(0, 10f)] private float radius;

    private int pairedObjectCount;
    private int totalObjectCount;

    private int CurrentLevel
    {
        get => PlayerPrefs.GetInt(nameof(CurrentLevel), 1);
        set => PlayerPrefs.SetInt(nameof(CurrentLevel), value);
    }
    
    private int initialObjectCount = 14;
    
    private void Start()
    {
        SpawnObjects();
        PlacementArea.OnAnyObjectsPaired += PlacementArea_OnAnyObjectsPaired;
    }

    private void PlacementArea_OnAnyObjectsPaired(bool isSuccessful)
    {
        if (!isSuccessful)
        {
            return;
        }
        
        pairedObjectCount++;

        if (pairedObjectCount >= totalObjectCount / 2)
        {
            CurrentLevel++;
            pairedObjectCount = 0;
            SpawnObjects();
        }
    }

    private void SpawnObjects()
    {
        totalObjectCount = initialObjectCount + 4 * (CurrentLevel - 1);
        StartCoroutine(SpawnObjectCoroutine(totalObjectCount));
    }

    private IEnumerator SpawnObjectCoroutine(int objectCount)
    {
        for (int i = 0; i < objectCount / 2; i++)
        {
            TouchManager.Instance.isTouchEnabled = false;
            int randomIndex = Random.Range(0, draggableObjectArray.Length);
            yield return new WaitForSeconds(0.2f);
            
            Instantiate(draggableObjectArray[randomIndex].prefab, GetRandomPosition(), GetRandomRotation());
            yield return new WaitForSeconds(0.2f);
            Instantiate(draggableObjectArray[randomIndex].prefab, GetRandomPosition(), GetRandomRotation());
        }
        
        TouchManager.Instance.isTouchEnabled = true;
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 randomPosition = Random.insideUnitSphere * radius;
        float randomYPosition = Random.Range(0.6f, 1f);

        randomPosition = new Vector3(randomPosition.x, randomYPosition, randomPosition.z);
        return randomPosition;
    }

    private Quaternion GetRandomRotation()
    {
        int randomXPosition = Random.Range(0, 360);
        int randomYPosition = Random.Range(0, 360);
        int randomZPosition = Random.Range(0, 360);
        
        Vector3 randomRotation = new Vector3(randomXPosition, randomYPosition, randomZPosition);
        return Quaternion.Euler(randomRotation);
    }
}
