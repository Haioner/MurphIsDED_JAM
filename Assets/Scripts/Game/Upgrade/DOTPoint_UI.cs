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
        dotText.SetText("<sprite index= 0>" + DataManager.instance.itemData.dotPoints.ToString());
    }
}
