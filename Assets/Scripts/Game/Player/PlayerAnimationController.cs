using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private PlayerManager playerManager;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    private void Update()
    {
        MovementAnimation();
        DashAnimation();
        DieAnimation();
    }

    private void MovementAnimation()
    {
        if(playerManager.playerState == PlayerState.Walk)
        {
            playerManager.anim.SetFloat("Input", 1);
        }
        else if(playerManager.playerState == PlayerState.Idle)
        {
            playerManager.anim.SetFloat("Input", 0);
        }
    }

    private void DashAnimation()
    {
        if (playerManager.playerState == PlayerState.Dash)
            playerManager.anim.SetBool("Dash", true);
        else
            playerManager.anim.SetBool("Dash", false);
    }

    private void DieAnimation()
    {
        if(playerManager.playerState == PlayerState.Die)
        {
            playerManager.anim.SetBool("Die", true);
        }
    }
}
