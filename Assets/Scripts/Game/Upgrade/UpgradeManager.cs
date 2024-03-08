using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [Header("CACHE")]
    public GameController gameController;
    public InventoryManager inventoryManager;

    [Header("Attacks")]
    [SerializeField] private int attackItemsCount = 4;
    [SerializeField] private UpgradeAttackItem upgradeAttackItem;
    [SerializeField] private Transform upgradeAttacksHolder;

    private List<UpgradeAttackItem> upgradeAttackItems = new List<UpgradeAttackItem>();
    private List<int> attackIndexList = new List<int>();
    public AttackSO SelectedAttack { set; get; }
    private int currentSelectedItem;

    private void OnEnable()
    {
        ResetUpgradeList();
        SkipUpgrades();
        CreateUpgradeAttackItems();
    }

    public void SkipUpgrades()
    {
        int maximizedSkills = 0;
        for (int i = 0; i < DataManager.instance.gameData.EqquipedAttacks.Count; i++)
        {
            PlayerAttacks attack = DataManager.instance.gameData.EqquipedAttacks[i];
            if (attack.AttackLevel >= attack.MaxAttackLevel)
                maximizedSkills++;
        }

        if (maximizedSkills >= 4)
        {
            DataManager.instance.gameData.UnlockNextGameLevel();
            DataManager.instance.gameData.NextGameLevel();
            gameController.ResetGameController();
            DataManager.instance.SaveData();
        }
    }

    public void UpdateSelectedItemColor(int itemIndex)
    {
        currentSelectedItem = itemIndex;
        for (int i = 0; i < upgradeAttackItems.Count; i++)
        {
            if(i == currentSelectedItem)
                upgradeAttackItems[i].SetItemNormalColor(Color.yellow);
            else
                upgradeAttackItems[i].SetItemNormalColor(Color.white);
        }
    }

    private void CreateUpgradeAttackItems()
    {
        for (int i = 0; i < attackItemsCount; i++)
        {
            UpgradeAttackItem item = Instantiate(upgradeAttackItem, upgradeAttacksHolder);
            item.InitiateItem(this, GetRandomAttackIndex(), i);
            upgradeAttackItems.Add(item);
        }
    }

    private int GetRandomAttackIndex()
    {
        int randIndex = Random.Range(0, DataManager.instance.gameData.Attacks.Count);
        while (attackIndexList.Contains(randIndex))
        {
            randIndex = Random.Range(0, DataManager.instance.gameData.Attacks.Count);
        }
        attackIndexList.Add(randIndex);

        return randIndex;
    }

    private void ResetUpgradeList()
    {
        if(upgradeAttackItems.Count > 0)
        {
            foreach (UpgradeAttackItem item in upgradeAttackItems)
            {
                Destroy(item.gameObject);
            }
            upgradeAttackItems.Clear();
            attackIndexList.Clear();
        }
    }
}
