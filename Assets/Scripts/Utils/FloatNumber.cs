using UnityEngine;
using TMPro;

public class FloatNumber : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI floatText;

    public void InitiateFloatNumber(float value, int colorIndex)
    {
        if(colorIndex == 0)
        {
            floatText.SetText("<color=#FF0000>" + value.ToString("F2")); //Red
        }
        else
        {
            floatText.SetText("<color=#00FF00>" + "+" + value.ToString("F2")); //Green
        }
    }

    public void DestroyDamage()
    {
        Destroy(gameObject);
    }
}
