using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [Header("CACHE")]
    public GameController gameController;
    public InventoryManager inventoryManager;
    public GameObject itemsHolder;
    public GameObject attacksHolder;
    public GameObject DotPointsHolder;
    public GameObject buyHolder;
    [SerializeField] private Transform upgradeHolder;

    [Header("Attacks")]
    [SerializeField] private int attackItemsCount = 4;
    [SerializeField] private UpgradeAttackItem upgradeAttackItem;

    private List<UpgradeAttackItem> upgradeAttackItems = new List<UpgradeAttackItem>();
    private List<int> attackIndexList = new List<int>();
    public AttackSO SelectedAttack { set; get; }
    private int currentSelectedAttackItem;

    [Header("Items")]
    [SerializeField] private ItemShop itemShop;
    private ItemShop currentItemShop;
    private ItemSO selectedItemSO;
    private int currentSelectedItemIndex;

    private void OnEnable()
    {
        ResetUpgradeList();
        ClearItemShop();

        SkipUpgrades();
        CreateUpgradeAttackItems();
        CreateItemShop();
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

    public void FinishUpgrade()
    {
        DataManager.instance.gameData.UnlockNextGameLevel();
        DataManager.instance.gameData.NextGameLevel();
        gameController.ResetGameController();

        attacksHolder.gameObject.SetActive(false);
        itemsHolder.gameObject.SetActive(false);
        DotPointsHolder.gameObject.SetActive(false);
        buyHolder.gameObject.SetActive(false);

        DataManager.instance.SaveData();
    }

    #region Attacks
    public void StartAttackSelection()
    {
        inventoryManager.SetActiveAttackSlotsSelection(true);
        attacksHolder.SetActive(true);

        itemsHolder.SetActive(false);
        buyHolder.SetActive(false);
    }

    public void UpdateSelectedItemColor(int itemIndex)
    {
        currentSelectedAttackItem = itemIndex;
        for (int i = 0; i < upgradeAttackItems.Count; i++)
        {
            if (i == currentSelectedAttackItem)
                upgradeAttackItems[i].SetItemSelectionSprite(false);
            else
                upgradeAttackItems[i].SetItemSelectionSprite(true);
        }
    }

    private void CreateUpgradeAttackItems()
    {
        for (int i = 0; i < attackItemsCount; i++)
        {
            UpgradeAttackItem item = Instantiate(upgradeAttackItem, upgradeHolder);
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
    #endregion

    #region Items

    public void CancelBuy()
    {
        currentItemShop.SetActiveBlackScreen(false);
    }

    public void StartBuyItem(ItemSO item, int itemIndex)
    {
        if (DataManager.instance.itemData.dotPoints >= item.ItemList[itemIndex].ItemPrice)
        {
            inventoryManager.UpdatePlayerStatsText();
            UpdateSelectedItemColor(-1);
            itemsHolder.SetActive(true);
            buyHolder.SetActive(true);

            attacksHolder.SetActive(false);

            selectedItemSO = item;
            currentSelectedItemIndex = itemIndex;
        }
    }

    public void BuyItemButton()
    {
        DataManager.instance.itemData.dotPoints -= selectedItemSO.ItemList[currentSelectedItemIndex].ItemPrice;
        DotPointsHolder.GetComponent<DOTPoint_UI>().UpdateDotTEXT();
        ReplaceEqquipedItem();
        FinishUpgrade();
    }

    private void ReplaceEqquipedItem()
    {
        switch (selectedItemSO.ItemList[currentSelectedItemIndex].itemType)
        {
            case ItemType.MeeleWeapon: DataManager.instance.itemData.CurrentWeaponIndex = currentSelectedItemIndex; break;
            case ItemType.Helmet: DataManager.instance.itemData.CurrentHelmetIndex = currentSelectedItemIndex; break;
            case ItemType.Chestplate: DataManager.instance.itemData.CurrentChestplateIndex = currentSelectedItemIndex; break;
            case ItemType.Boots: DataManager.instance.itemData.CurrentBootsIndex = currentSelectedItemIndex; break;
        }
        DataManager.instance.gameData.UpdateStats(DataManager.instance.itemData);
    }

    private void CreateItemShop()
    {
        DotPointsHolder.SetActive(true);

        ItemSO randItem = GetRandomItemSO();
        ItemShop item = Instantiate(itemShop, upgradeHolder);
        item.InitiateItem(randItem, this, GetRandomItemIndex(randItem));
        currentItemShop = item;
    }

    private ItemSO GetRandomItemSO()
    {
        int randListItem = Random.Range(0, 4);
        ItemSO item;
        switch (randListItem)
        {
            default: item = DataManager.instance.itemData.MeeleWeapons; break;
            case 0: item = DataManager.instance.itemData.MeeleWeapons; break;
            case 1: item = DataManager.instance.itemData.Helmet; break;
            case 2: item = DataManager.instance.itemData.Chestplate; break;
            case 3: item = DataManager.instance.itemData.Boots; break;
        }

        return item;
    }

    private int GetRandomItemIndex(ItemSO itemSO)
    {
        return Random.Range(0, itemSO.ItemList.Count);
    }

    private void ClearItemShop()
    {
        if (currentItemShop != null)
            Destroy(currentItemShop.gameObject);
        currentItemShop = null;
    }
    #endregion
}
