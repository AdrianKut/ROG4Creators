using System.Collections;
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

    private void Awake()
    {
        StartCoroutine(SpawnEnemies());
        InvokeRepeating("SpawnObstacles", spawnObstaclesDelay, spawnObstaclesInterval);
    }

    public static float spawnDelay = 2.5f;
    IEnumerator SpawnEnemies()
    {
        int index;
        while (true)
        {
            index = Random.Range(0, 2);
            MonsterToSpawn(index);

            yield return new WaitForSeconds(spawnDelay);

            index = Random.Range(0, gameObjectsMonstersToSpawn.Length);
            MonsterToSpawn(index);

            yield return new WaitForSeconds(spawnDelay);

            index = Random.Range(0, 2);
            MonsterToSpawn(index);

            yield return new WaitForSeconds(spawnDelay);

            index = Random.Range(0, 4);
            MonsterToSpawn(index);

            yield return new WaitForSeconds(spawnDelay);

            index = Random.Range(0, 4);
            MonsterToSpawn(index);

            yield return new WaitForSeconds(spawnDelay);

            index = Random.Range(0, gameObjectsMonstersToSpawn.Length);
            MonsterToSpawn(index);
        }
    }

    private void MonsterToSpawn(int index)
    {
        var _ =  Instantiate(gameObjectsMonstersToSpawn[index], new Vector3(Random.Range(7f, 13f),
                        gameObjectsMonstersToSpawn[index].transform.position.y, 0f),
                        gameObjectsMonstersToSpawn[index].transform.rotation);

        
        //Ustawiæ ich prêdkoœæ w zale¿noœci od odleg³oœci
        _.gameObject.GetComponent<Enemy>().SetSpeed(100);
    }

    void SpawnObstacles() => SpawnObjects(gameObjectsObstaclesToSpawn);

    private void SpawnObjects(GameObject[] gameObjectToSpawn)
    {
        int index = Random.Range(0, gameObjectToSpawn.Length);

        if (!GameManager.gameManagerInstance.isGameOver)
        {
            var gameObject = Instantiate(gameObjectToSpawn[index], new Vector3(Random.Range(7f, 12f),
                gameObjectToSpawn[index].transform.position.y, 0f),
                gameObjectToSpawn[index].transform.rotation);
        }
    }


}
