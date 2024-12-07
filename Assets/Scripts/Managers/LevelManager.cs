using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private DraggableObjectSO[] draggableObjectArray;
    [SerializeField] [Range(0, 10f)] private float radius;

    private int CurrentLevel
    {
        get => PlayerPrefs.GetInt(nameof(CurrentLevel), 1);
        set => PlayerPrefs.SetInt(nameof(CurrentLevel), value);
    }
    
    private int initialObjectCount = 14;
    
    private void Start()
    {
        SpawnObjects();
    }

    public void SpawnObjects()
    {
        StartCoroutine(SpawnObjectCoroutine(initialObjectCount + 4 * (CurrentLevel - 1)));
    }

    private IEnumerator SpawnObjectCoroutine(int objectCount)
    {
        TouchManager.Instance.isTouchEnabled = false;
        
        for (int i = 0; i < objectCount / 2; i++)
        {
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
        float randomYPosition = Random.Range(0.4f, 0.7f);

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
