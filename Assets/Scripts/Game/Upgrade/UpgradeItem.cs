using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpgradeType
{
    MaxHealth, HealthRegen, DamageMultiplier
}

public class UpgradeItem : MonoBehaviour
{
    [SerializeField] private UpgradeType upgradeType;
    private GameController gameController;

    public void InitiateItem(GameController controller)
    {
        gameController = controller;
    }

    public void SelectUpgradeItem()
    {
        DataManager.instance.gameData.UnlockNextGameLevel();
        DataManager.instance.gameData.NextGameLevel();
        gameController.ResetGameController();
    }
}
