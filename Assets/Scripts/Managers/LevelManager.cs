using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public static Action OnAnyObjectSpawned;

    /*[HideInInspector]*/ public List<DraggableObject> activeDraggableObjectList = new();
    
    [SerializeField] private DraggableObjectSO[] draggableObjectArray;
    [SerializeField] [Range(0, 10f)] private float radius;

    private int CurrentLevel
    {
        get => PlayerPrefs.GetInt(nameof(CurrentLevel), 1);
        set => PlayerPrefs.SetInt(nameof(CurrentLevel), value);
    }
    
    private const int INITIAL_OBJECT_COUNT = 14;
    
    private int pairedObjectCount;
    private int totalObjectCount;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        SpawnObjects();
        PlacementArea.OnAnyObjectsPaired += PlacementArea_OnAnyObjectsPaired;
        DraggableObject.OnBeingDestroyed += DraggableObject_OnBeingDestroyed;
    }

    private void DraggableObject_OnBeingDestroyed(DraggableObject destroyedDraggableObject)
    {
        var index = activeDraggableObjectList.IndexOf(destroyedDraggableObject);
        activeDraggableObjectList.RemoveAt(index);
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
        totalObjectCount = INITIAL_OBJECT_COUNT + 4 * (CurrentLevel - 1);
        StartCoroutine(SpawnObjectCoroutine(totalObjectCount));
    }

    private IEnumerator SpawnObjectCoroutine(int objectCount)
    {
        TouchManager.Instance.isTouchEnabled = false;
        
        for (int i = 0; i < objectCount / 2; i++)
        {
            int randomIndex = Random.Range(0, draggableObjectArray.Length);
            
            for (int j = 0; j < 2; j++)
            {
                yield return new WaitForSeconds(0.2f);
                
                SpawnObjectAtPosition(draggableObjectArray[randomIndex].prefab, GetRandomPosition());
            }
            
        }
        
        TouchManager.Instance.isTouchEnabled = true;
    }

    public DraggableObject SpawnObjectAtPosition(GameObject prefab, Vector3 position)
    {
        var spawnedObject = Instantiate(prefab, position, GetRandomRotation());
        var spawnedDraggableObject = spawnedObject.GetComponent<DraggableObject>();
        activeDraggableObjectList.Add(spawnedDraggableObject);
        OnAnyObjectSpawned?.Invoke();
        return spawnedDraggableObject;
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

    private void OnDestroy()
    {
        PlacementArea.OnAnyObjectsPaired -= PlacementArea_OnAnyObjectsPaired;
        DraggableObject.OnBeingDestroyed -= DraggableObject_OnBeingDestroyed;
    }
}
