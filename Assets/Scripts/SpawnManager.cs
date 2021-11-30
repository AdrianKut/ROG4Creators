using System.Collections;
using UnityEngine;
using System.Linq;

public class SpawnManager : MonoBehaviour
{

    [Header("Obstacles")]
    [SerializeField]
    GameObject[] gameObjectsObstaclesToSpawn;
    public static float spawnObstacleDelay = 2.5f;

    [Header("Monsters")]
    [SerializeField]
    GameObject[] gameObjectsMonstersToSpawn;
    public static float spawnMonsterDelay = 2.5f;

    private PowerUpManager powerUpManager;
    private void Awake()
    {
        //Starting speed of obstacles
        gameObjectsObstaclesToSpawn.ToList().ForEach(x => x.GetComponent<Enemy>().SetSpeed(10));

        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnObstacles());

        powerUpManager = PowerUpManager.PowerUpManagerInstance;
        powerUpManager.OnSlowMotionActivated.AddListener(DecreaseSpeedObjects);
        powerUpManager.OnSlowMotionDeactivated.AddListener(IncreaseSpeedObjects);
    }

    private void DecreaseSpeedObjects()
    {
        var objects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var item in objects)
        {
            var _enemy = item.GetComponent<Enemy>();
            var _speed = _enemy.GetSeed();
            _enemy.SetSpeed(_speed / 2);
        }


        foreach (var item in gameObjectsObstaclesToSpawn)
        {
            var _enemy = item.GetComponent<Enemy>();
            var _speed = _enemy.GetSeed();
            _enemy.SetSpeed(_speed / 2);
        }


        foreach (var item in gameObjectsMonstersToSpawn)
        {
            var _enemy = item.GetComponent<Enemy>();
            var _speed = _enemy.GetSeed();
            _enemy.SetSpeed(_speed / 2);
        }
    }

    private void IncreaseSpeedObjects()
    {
        var objects = GameObject.FindGameObjectsWithTag("Enemy");
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

            yield return new WaitForSeconds(spawnMonsterDelay);

            index = Random.Range(0, gameObjectsMonstersToSpawn.Length);
            ObjectToSpawn(index);

            yield return new WaitForSeconds(spawnMonsterDelay);

            index = Random.Range(0, 2);
            ObjectToSpawn(index);

            yield return new WaitForSeconds(spawnMonsterDelay);

            index = Random.Range(0, 4);
            ObjectToSpawn(index);

            yield return new WaitForSeconds(spawnMonsterDelay);

            index = Random.Range(0, 4);
            ObjectToSpawn(index);

            yield return new WaitForSeconds(spawnMonsterDelay);

            index = Random.Range(0, gameObjectsMonstersToSpawn.Length);
            ObjectToSpawn(index);
        }
    }

    private void ObjectToSpawn(int index)
    {
        var _ = Instantiate(gameObjectsMonstersToSpawn[index], new Vector3(Random.Range(7f, 13f),
                        gameObjectsMonstersToSpawn[index].transform.position.y, 0f),
                        gameObjectsMonstersToSpawn[index].transform.rotation);
    }

    IEnumerator SpawnObstacles()
    {     
        int cycleCounter = 0;

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(spawnObstacleDelay,4f));

            int index = Random.Range(0, gameObjectsObstaclesToSpawn.Length);
            if (!GameManager.GameManagerInstance.isGameOver)
            {
                var gameObject = Instantiate(gameObjectsObstaclesToSpawn[index], new Vector3(Random.Range(7f, 12f),
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
