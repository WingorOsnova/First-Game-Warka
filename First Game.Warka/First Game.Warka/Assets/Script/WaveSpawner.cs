using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct Wave
    {
        public Enemy[] enmies;
        public int count;
        public float timeBtwSpawn;

    }

    [SerializeField] Wave[] waves;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] float timeBtwWaves;

    Wave currentwWave;
    [HideInInspector]public int currentwWaveIndex;
    Transform player;

    bool isSpawnerFinished = false;

    [SerializeField] TextMeshProUGUI waveText;
    bool isFreeTime = true;
    float curttimeBtwWaves;

    [SerializeField] GameObject spawnEffect;

    private void Start()
    {
        player = Player.instance.transform;
        curttimeBtwWaves = timeBtwWaves;
        UpdataText();
        StartCoroutine(CallNextWave(currentwWaveIndex));
    }

    private void Update()
    {
        UpdataText();

        if (isSpawnerFinished && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        { 
            isSpawnerFinished = false ;
            if (currentwWaveIndex + 1 < waves.Length)
            {
               
                currentwWaveIndex++;
                StartCoroutine(CallNextWave(currentwWaveIndex));
            }
            else
            {
               //Создание босса
            }
        }
    }

    void UpdataText()
    {
        if (isFreeTime) waveText.text = "Следующия волна через:" + ((int)(curttimeBtwWaves -= Time.deltaTime)).ToString();                   
        else waveText.text = "Волна:" + currentwWaveIndex.ToString();

    }
    IEnumerator CallNextWave(int waveIndex)
    {
        curttimeBtwWaves = timeBtwWaves;
        isFreeTime = true ; 
        yield return new WaitForSeconds(timeBtwWaves);
        isFreeTime = false ;
        StartCoroutine(SpawWave(waveIndex));
    }

    IEnumerator SpawWave(int waveIndex)
    { 
       currentwWave = waves[waveIndex];
        for (int i = 0; i < currentwWave.count; i++)
        {
            if (player == null) yield break;
            Enemy randomEnemy = currentwWave.enmies[Random.Range(0, currentwWave.enmies.Length)];
            Transform randomSpawnPont = spawnPoints[Random.Range(0, spawnPoints.Length)];

            Instantiate(randomEnemy, randomSpawnPont.position, Quaternion.identity);
            Instantiate(spawnEffect, randomSpawnPont.position, Quaternion.identity);

            if (i == currentwWave.count - 1)
            { 
                isSpawnerFinished |= true;
            }
            else
            { 
                 isSpawnerFinished = false ;
            }
            yield return new WaitForSeconds(currentwWave.timeBtwSpawn);
        }
    }
}
