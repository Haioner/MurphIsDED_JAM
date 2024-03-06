using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveList")]
public class WaveSO : ScriptableObject
{
    public List<EnemySO> enemiesList = new List<EnemySO>();
}
