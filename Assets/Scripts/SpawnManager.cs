using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Obstacles")]
    [SerializeField]
    GameObject[] gameObjectsObstaclesToSpawn;

    [SerializeField]
    private float spawnObstaclesDelay;
    
    [SerializeField]
    private float spawnObstaclesInterval;


    [Header("Monsters")]
    [SerializeField]
    GameObject[] gameObjectsMonstersToSpawn;

    [SerializeField]
    private float spawnMonstersDelay;

    [SerializeField]
    private float spawnMonstersInterval;


    void Start()
    {
        InvokeRepeating("SpawnMonsters", spawnMonstersDelay, spawnMonstersInterval);
        InvokeRepeating("SpawnObstacles", spawnObstaclesDelay, spawnObstaclesInterval);
    }

    void SpawnMonsters() => SpawnObjects(gameObjectsMonstersToSpawn);
    void SpawnObstacles() => SpawnObjects(gameObjectsObstaclesToSpawn);
  

    private void SpawnObjects(GameObject[] gameObjectToSpawn)
    {
        int index = Random.Range(0, gameObjectToSpawn.Length);

        if (!GameManager.gameManagerInstance.isGameOver)
        {
            var gameObject = Instantiate(gameObjectToSpawn[index], new Vector3(Random.Range(7f, 13f), gameObjectToSpawn[index].transform.position.y, 0f), gameObjectToSpawn[index].transform.rotation);
            //gameObject.GetComponent<Enemy>()

        }
    }


}
