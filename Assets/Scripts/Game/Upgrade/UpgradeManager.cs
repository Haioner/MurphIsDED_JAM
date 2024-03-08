using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private GameController controller;
    [SerializeField] private UpgradeItem upgradeItem;
    [SerializeField] private Transform upgradeItemsHolder;
    private List<UpgradeItem> upgradeItems = new List<UpgradeItem>();

    private void OnEnable()
    {
        ResetUpgradeList();
        CreateUpgradeItems();
    }

    private void CreateUpgradeItems()
    {
        for (int i = 0; i < 3; i++)
        {
            UpgradeItem item = Instantiate(upgradeItem, upgradeItemsHolder);
            item.InitiateItem(controller);
            upgradeItems.Add(item);
        }
    }

    private void ResetUpgradeList()
    {
        if(upgradeItems.Count > 0)
        {
            foreach (UpgradeItem item in upgradeItems)
            {
                Destroy(item.gameObject);
            }
            upgradeItems.Clear();
        }
    }
}
