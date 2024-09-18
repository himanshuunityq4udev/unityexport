using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomeObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefabToSpawn;

    [Header("X-Direction")]
    [SerializeField] private float minXDistance = 1;
    [SerializeField] private float maxXDistance = 5;
    [Header("Z-Direction")]

    [SerializeField] private float minZDistance = 1;
    [SerializeField] private float maxZDistance = 5;
    
    [Header("Y-Direction")]
    [SerializeField] private float minYDistance = 1;
    [SerializeField] private float maxYDistance = 5;

    [Range(0, 500)]
    [SerializeField] private float NumberOfSpawnObject = 5;

    private void Start()
    {
        SpawnObjectAtRamdome();
    }

    private void SpawnObjectAtRamdome()
    {
        while (NumberOfSpawnObject >= 0)
        {
            
            SpawnObject(RandomPositionGenerator(), prefabToSpawn);
            
            NumberOfSpawnObject--;
        }

    }

    void SpawnObject(Vector3 spawnPosition, GameObject _prefabToSpawn)
    {
        Instantiate(_prefabToSpawn,spawnPosition,Quaternion.identity);
    }

    Vector3 RandomPositionGenerator()
    {
        float xDistance = Random.Range(minXDistance, maxXDistance);
        float zDistance = Random.Range(minZDistance, maxZDistance);
        float yDistance = Random.Range(minYDistance, maxYDistance);

        Vector3 randamPosition = new Vector3(xDistance, yDistance, zDistance);
        return randamPosition;
    }

}
