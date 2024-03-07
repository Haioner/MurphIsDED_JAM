using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveEnemy
{
    public EnemySO enemy;
    public float enemySpawnCooldown = 1f;
}

[CreateAssetMenu(fileName = "WaveList")]
public class WaveSO : ScriptableObject
{
    //public List<EnemySO> enemiesList = new List<EnemySO>();
    public List<WaveEnemy> enemiesList = new List<WaveEnemy>();
}
