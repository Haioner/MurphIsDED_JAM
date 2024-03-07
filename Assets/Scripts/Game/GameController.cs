using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject player;

    [Header("CACHE")]
    [SerializeField] private EnemyManager enemyManager;

    [Header("Spawn")]
    [SerializeField] private WaveSO waveSO;
    [SerializeField] private Vector2 minMaxSpawnRadiusX;
    [SerializeField] private Vector2 minMaxSpawnRadiusY;
    private float currentSpawnRate;
    private int currentEnemyInWave;

    [Header("Wave")]
    [SerializeField] private TextMeshProUGUI remainingText;
    private int remainingEnemies;

    private void Start()
    {
        currentSpawnRate = waveSO.enemiesList[currentEnemyInWave].enemySpawnCooldown;
        remainingEnemies = waveSO.enemiesList.Count;
        SetRemainingText();
    }

    private void Update()
    {
        UpdateSpawnRate();
    }

    private void SetRemainingText()
    {
        remainingText.SetText(remainingEnemies.ToString());
    }

    public void SubtractEnemyRemaining()
    {
        remainingEnemies--;
        SetRemainingText();
    }

    #region Spawner

    private void UpdateSpawnRate()
    {
        if (currentEnemyInWave >= waveSO.enemiesList.Count) return;

        currentSpawnRate -= Time.deltaTime;
        if(currentSpawnRate <= 0)
        {
            SpawnEnemy();
            currentEnemyInWave++;
            if (currentEnemyInWave < waveSO.enemiesList.Count)
                currentSpawnRate = waveSO.enemiesList[currentEnemyInWave].enemySpawnCooldown;
        }
    }

    private void SpawnEnemy()
    {
        float spawnPositionX = Random.Range(minMaxSpawnRadiusX.x, minMaxSpawnRadiusX.y);
        float spawnPositionY = Random.Range(minMaxSpawnRadiusY.x, minMaxSpawnRadiusY.y);
        Vector2 spawnPosition = new Vector2(spawnPositionX, spawnPositionY);
        EnemyManager enemy = Instantiate(enemyManager, spawnPosition, Quaternion.identity);

        UnityEvent dieEventToAdd = new UnityEvent();
        dieEventToAdd.AddListener(SubtractEnemyRemaining);
        enemy.InitiateEnemy(waveSO.enemiesList[currentEnemyInWave].enemy, player, dieEventToAdd);
    }

    #endregion
}
