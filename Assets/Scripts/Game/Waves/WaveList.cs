using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Wave/WaveList")]
public class WaveList : ScriptableObject
{
    public List<WaveSO> waveList = new List<WaveSO>();
}
