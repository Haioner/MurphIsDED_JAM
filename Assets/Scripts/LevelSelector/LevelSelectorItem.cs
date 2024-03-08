using UnityEngine;
using UnityEngine.UI;

public class LevelSelectorItem : MonoBehaviour
{
    public int id {  get; set; }
    private bool isUnlocked;

    private void Awake()
    {
        GetComponent<Button>().interactable = isUnlocked;
    }

    public void UnlockLevel()
    {
        isUnlocked = true;
        GetComponent<Button>().interactable = isUnlocked;
    }
}
