using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum PlayerState
{
    Idle, Attack, Die, Walk, Run, Dash
}

public class PlayerManager : MonoBehaviour
{
    public PlayerState playerState;
    [SerializeField] private HealthController healthController;
    [HideInInspector] public Animator anim;
    private GameData gameData;

    private void Start()
    {
        anim = GetComponent<Animator>();
        gameData = DataManager.instance.gameData;
        healthController.SetMaxHealth(gameData.Health);
    }
}
