using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject player;
    private HealthController playerHealth;

    [Header("CACHE")]
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private GameObject winHolder;
    [SerializeField] private GameObject loseHolder;

    [Header("Spawn")]
    [SerializeField] private WaveSO waveSO;
    [SerializeField] private WaveList waveList;
    [SerializeField] private Vector2 minMaxSpawnRadiusX;
    [SerializeField] private Vector2 minMaxSpawnRadiusY;
    private float currentSpawnRate;
    private int currentEnemyInWave;

    [Header("Wave")]
    [SerializeField] private TextMeshProUGUI remainingText;
    private int remainingEnemies;

    private void Start()
    {
        playerHealth = player.GetComponentInChildren<HealthController>();
        ResetGameController();
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
        WinWave();
    }

    public void ResetGameController()
    {
        if (CheckWaveScene()) return;
        waveSO = waveList.waveList[DataManager.instance.gameData.CurrentGameLevel];
        currentEnemyInWave = 0;
        currentSpawnRate = waveSO.enemiesList[currentEnemyInWave].enemySpawnCooldown;
        remainingEnemies = waveSO.enemiesList.Count;
        SetRemainingText();
        winHolder.SetActive(false);
        playerHealth.ResetCurrentHealth();
    }

    private bool CheckWaveScene()
    {
        string waveSceneName = waveList.waveList[DataManager.instance.gameData.CurrentGameLevel].SceneName;

        Scene scene = SceneManager.GetActiveScene();
        string currentSceneName = scene.name;
        if (waveSceneName != currentSceneName)
        {
            TransitionController.instance.TransitionToSceneName(waveSceneName);
            return true;
        }
        return false;
    }

    #region Win or Lose

    public void BackToMenu()
    {
        Time.timeScale = 1;
        TransitionController.instance.TransitionToSceneName("LevelSelector");
    }

    public void ReloadScene()
    {
        Time.timeScale = 1;
        Scene scene = SceneManager.GetActiveScene();
        string currentSceneName = scene.name;
        TransitionController.instance.TransitionToSceneName(currentSceneName);
    }

    private void WinWave()
    {
        if (remainingEnemies <= 0)
        {
            winHolder.SetActive(true);
        }
    }

    public void LoseWave()
    {
        if (remainingEnemies <= 0) return;
        loseHolder.SetActive(true);
    }

    #endregion

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
