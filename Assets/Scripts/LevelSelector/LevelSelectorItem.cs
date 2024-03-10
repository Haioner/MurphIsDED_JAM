using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectorItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveTEXT;
    private bool isUnlocked;

    public void UpdateSelectorItem(int wave)
    {
        GetComponent<Button>().interactable = isUnlocked;
        waveTEXT.text = "Wave " + wave.ToString();
    }

    public void UnlockLevel()
    {
        isUnlocked = true;
        GetComponent<Button>().interactable = isUnlocked;
    }
}
