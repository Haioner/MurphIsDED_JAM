using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinLoader : MonoBehaviour
{
    [SerializeField] private SpriteRenderer weapon;
    [SerializeField] private SpriteRenderer helmet;
    [SerializeField] private SpriteRenderer chestplate;
    [SerializeField] private SpriteRenderer[] boots;

    private void Start()
    {
        UpdateSkin();
    }

    public void UpdateSkin()
    {
        if (DataManager.instance.itemData.CurrentWeaponIndex >= 0)
            UpdatePart(weapon, DataManager.instance.itemData.MeeleWeapons.ItemList[DataManager.instance.itemData.CurrentWeaponIndex].Icon);

        if (DataManager.instance.itemData.CurrentHelmetIndex >= 0)
            UpdatePart(helmet, DataManager.instance.itemData.Helmet.ItemList[DataManager.instance.itemData.CurrentHelmetIndex].Icon);

        if (DataManager.instance.itemData.CurrentChestplateIndex >= 0)
            UpdatePart(chestplate, DataManager.instance.itemData.Chestplate.ItemList[DataManager.instance.itemData.CurrentChestplateIndex].Icon);

        if (DataManager.instance.itemData.CurrentBootsIndex >= 0)
        {
            UpdatePart(boots[0], DataManager.instance.itemData.Boots.ItemList[DataManager.instance.itemData.CurrentBootsIndex].Icon);
            UpdatePart(boots[1], DataManager.instance.itemData.Boots.ItemList[DataManager.instance.itemData.CurrentBootsIndex].Icon);

        }
    }

    private void UpdatePart(SpriteRenderer renderer, Sprite sprite)
    {
        if (renderer != null)
            renderer.sprite = sprite;
    }
}
