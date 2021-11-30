using System.Collections;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [Header("Obstacles")]
    [SerializeField]
    GameObject[] gameObjectsObstaclesToSpawn;

    public float spawnObstacleDelay = 2.5f;

    [Header("Monsters")]
    [SerializeField]
    GameObject[] gameObjectsMonstersToSpawn;

    public float spawnMonsterDelay = 2.5f;


    private PowerUpManager powerUpManager;
    private void Awake()
    {
        StartingSpeedOfObjects();

        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnObstacles());

        powerUpManager = PowerUpManager.PowerUpManagerInstance;
        powerUpManager.OnSlowMotionActivated.AddListener(DecreaseSpeedObjects);
        powerUpManager.OnSlowMotionDeactivated.AddListener(IncreaseSpeedObjects);
    }

    private void StartingSpeedOfObjects()
    {
        gameObjectsObstaclesToSpawn.ToList().ForEach(x => x.GetComponent<Enemy>().SetSpeed(5f));

        gameObjectsMonstersToSpawn[0].GetComponent<Enemy>().SetSpeed(5f); // Enemy_1
        gameObjectsMonstersToSpawn[1].GetComponent<Enemy>().SetSpeed(3f); // Enemy_2
        gameObjectsMonstersToSpawn[2].GetComponent<Enemy>().SetSpeed(1f); // Enemy_3
        gameObjectsMonstersToSpawn[3].GetComponent<Enemy>().SetSpeed(1f); // Enemy_5
        gameObjectsMonstersToSpawn[4].GetComponent<Enemy>().SetSpeed(0.5f); // Enemy_4
    }


    private void DecreaseSpeedObjects()
    {
        //Objects on scene
        var objects = GameObject.FindObjectsOfType<Enemy>();
        foreach (var item in objects)
        {
            var _enemy = item.GetComponent<Enemy>();
            var _speed = _enemy.GetSeed();
            _enemy.SetSpeed(_speed / 2);
        }

        //Prefab Obstacles
        foreach (var item in gameObjectsObstaclesToSpawn)
        {
            var _enemy = item.GetComponent<Enemy>();
            var _speed = _enemy.GetSeed();
            _enemy.SetSpeed(_speed / 2);
        }

        //Prefab Monsters
        foreach (var item in gameObjectsMonstersToSpawn)
        {
            var _enemy = item.GetComponent<Enemy>();
            var _speed = _enemy.GetSeed();
            _enemy.SetSpeed(_speed / 2);
        }

    }

    private void IncreaseSpeedObjects()
    {
        var objects = GameObject.FindObjectsOfType<Enemy>();
        foreach (var item in objects)
        {
            var _enemy = item.GetComponent<Enemy>();
            var _speed = _enemy.GetSeed();
            _enemy.SetSpeed(_speed * 2);
        }

        foreach (var item in gameObjectsObstaclesToSpawn)
        {
            var _enemy = item.GetComponent<Enemy>();
            var _speed = _enemy.GetSeed();
            _enemy.SetSpeed(_speed * 2);
        }


        foreach (var item in gameObjectsMonstersToSpawn)
        {
            var _enemy = item.GetComponent<Enemy>();
            var _speed = _enemy.GetSeed();
            _enemy.SetSpeed(_speed * 2);
        }
    }


    IEnumerator SpawnEnemies()
    {
        int index;
        while (true)
        {
            index = Random.Range(0, 2);
            ObjectToSpawn(index);

            yield return new WaitForSeconds(spawnMonsterDelay); // 2.5s

            index = Random.Range(0, gameObjectsMonstersToSpawn.Length);
            ObjectToSpawn(index);

            yield return new WaitForSeconds(spawnMonsterDelay); // 2.5s

            index = Random.Range(0, 2);
            ObjectToSpawn(index);

            yield return new WaitForSeconds(spawnMonsterDelay); // 2.5s

            index = Random.Range(0, 4);
            ObjectToSpawn(index);

            yield return new WaitForSeconds(spawnMonsterDelay); // 2.5s

            index = Random.Range(0, 4);
            ObjectToSpawn(index);
            
            yield return new WaitForSeconds(spawnMonsterDelay); // 2.5s

            index = Random.Range(0, gameObjectsMonstersToSpawn.Length);
            ObjectToSpawn(index);


            // == 12.5s
            foreach (var item in gameObjectsMonstersToSpawn)
            {
                var _enemy = item.GetComponent<Enemy>();
                var _speed = _enemy.GetSeed();
                _enemy.SetSpeed(_speed += 0.05f);
            }

            if (spawnMonsterDelay >= 2f)
                spawnMonsterDelay -= 0.025f;
        }
    }

    private void ObjectToSpawn(int index)
    {
        var _ = Instantiate(gameObjectsMonstersToSpawn[index], new Vector3(Random.Range(7.5f, 16f),
                        gameObjectsMonstersToSpawn[index].transform.position.y, 0f),
                        gameObjectsMonstersToSpawn[index].transform.rotation);
    }

    IEnumerator SpawnObstacles()
    {
        int cycleCounter = 0;

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(spawnObstacleDelay, 5f));

            int index = Random.Range(0, gameObjectsObstaclesToSpawn.Length);
            if (!GameManager.GameManagerInstance.isGameOver)
            {
                var gameObject = Instantiate(gameObjectsObstaclesToSpawn[index], new Vector3(Random.Range(7.5f, 13f),
                    gameObjectsObstaclesToSpawn[index].transform.position.y, 0f),
                    gameObjectsObstaclesToSpawn[index].transform.rotation);
            }

            if (cycleCounter % 5 == 0 && cycleCounter != 0)
            {
                foreach (var item in gameObjectsObstaclesToSpawn)
                {
                    var _enemy = item.GetComponent<Enemy>();
                    var _speed = _enemy.GetSeed();
                    _enemy.SetSpeed(_speed += 0.5f);
                }
            }
            cycleCounter++;
        }
    }
}
