using UnityEngine;
using TMPro;

public class DOTPoint_UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dotText;

    private void Start()
    {
        UpdateDotTEXT();
    }

    public void UpdateDotTEXT()
    {
        dotText.SetText(DataManager.instance.itemData.dotPoints.ToString());
    }
}
