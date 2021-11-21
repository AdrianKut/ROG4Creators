using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class WaveConfig{
    public float timeSpawn;
    public int[] numberEnemy;
}

public class WaveManager : MonoBehaviour {
    [Header ("Waves Configuration")]
    [SerializeField]
    private  WaveConfig[] waves;
    [Header ("House of Enemies Configuration")]
    [SerializeField]
    private  Transform[] spawnPositions;
    [Header ("Enemies Configuration")]
    [SerializeField]
    private  GameObject[] enemies;
    [Header ("Other Configurations")]
    [SerializeField]
    private  TMP_Text currentWaveNumber;

    [SerializeField]
    private Animator currentWaveAnimator;
    [SerializeField]
    private  AudioSource nextWaveSoundEffect;

    private int waveEnemies;
    private int waveIndex;
    private Coroutine waveCoroutine;

    private static readonly int Play = Animator.StringToHash("Play");
    private static readonly int Lose = Animator.StringToHash("Lose");

    private void Start () {
        waveCoroutine = StartCoroutine(nameof(WaveController));
        currentWaveAnimator.SetBool(Lose, false);
        waveIndex = 0;
    }

    private IEnumerator WaveController() {
        while( waveIndex < waves.Length){
            foreach (var enemy in waves[waveIndex].numberEnemy)
            {
                yield return new WaitForSeconds(waves[waveIndex].timeSpawn);
                SpawnEnemy(enemy);
                waveEnemies++;
            }
            nextWaveSoundEffect.Play();
            waveIndex++;
            currentWaveNumber.text = $"Wave {waveIndex}";
            currentWaveAnimator.SetTrigger(Play);

            yield return new WaitUntil(() => waveEnemies == 0);
        }
    }

    public void EnemyDead()
    {
        waveEnemies--;
    }

    public void PlayerDead()
    {
        currentWaveNumber.text = $"YOU LOSE";
        currentWaveAnimator.SetBool(Lose, true);
        currentWaveAnimator.SetTrigger(Play);
        StopCoroutine(waveCoroutine);
        Time.timeScale = 0;
    }

    private void SpawnEnemy(int numberEnemy){
        float spawnPoint = Random.Range(0, spawnPositions.Length);
        var go = Instantiate(enemies[numberEnemy], spawnPositions[(int)spawnPoint].position, Quaternion.identity);
        go.transform.Rotate(0, spawnPoint == 0 ? 180 : 0, 0);
    }
}