using UnityEngine;
using TMPro;

public class DamageController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damageText;

    public void SetDamage(float damage)
    {
        damageText.SetText(damage.ToString());
    }

    public void DestroyDamage()
    {
        Destroy(gameObject);
    }
}
