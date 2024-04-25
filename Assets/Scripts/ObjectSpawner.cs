using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objectsToSpawn;

    private GameObject[] _spawnedObjects;
    
    [SerializeField]
    private GameObject[] spawnPoints;

    private bool shouldSpawnItems = true;
    void Start()
    {
        if (objectsToSpawn == null || spawnPoints == null)
        {
            shouldSpawnItems = false;
        }

        if (shouldSpawnItems)
        {
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                SpawnObject();
            }
        }
    }

    public void SpawnObject()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            int randomObjectIndex = Random.Range(0, objectsToSpawn.Length);
            SpawnPoint spawnPoint = spawnPoints[i].GetComponent<SpawnPoint>();
            if (!spawnPoint.bHasObject)
            {
                spawnPoint.bHasObject = true;
        
                GameObject spawnedObject = Instantiate(objectsToSpawn[randomObjectIndex], spawnPoints[i].transform.position, Quaternion.identity);
            }
        }
    }
}
