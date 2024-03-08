using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveEnemy
{
    public EnemySO enemy;
    public float enemySpawnCooldown = 1f;
}

[CreateAssetMenu(menuName = "Wave/WaveSO")]
public class WaveSO : ScriptableObject
{
    public string SceneName;
    public List<WaveEnemy> enemiesList = new List<WaveEnemy>();
}
