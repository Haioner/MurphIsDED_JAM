using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    public UnityEvent DieEvent;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private float progressSpeed = 4.2f;

    [Header("Damage")]
    [SerializeField] private DamageController damageController;
    public UnityEvent DamageEvent;

    [Header("CanvasGroup")]
    [SerializeField] private bool canUpdateCG = true;
    [SerializeField] private CanvasGroup cg;
    private bool canVisibilityDown;
    private float visibilitySpeed = 0.5f;

    private void Start()
    {
        currentHealth = maxHealth;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = healthSlider.maxValue;
        UpdateHealthText();
    }

    private void Update()
    {
        UpdateHealthSlider();
        CanvasVisibility();

        if (Input.GetKeyDown(KeyCode.F))
            Damage(1);
        if (Input.GetKeyDown(KeyCode.G))
            Damage(10);
    }

    public void SetMaxHealth(float newValue)
    {
        maxHealth = newValue;
    }

    public void Damage(float damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            SpawnDamageCanvas(damage);
            StartCoroutine(nameof(DamageVisibility));
            DamageEvent?.Invoke();
        }
        else 
            currentHealth = 0;

        UpdateHealthText();
        Die();
    }

    IEnumerator DamageVisibility()
    {
        cg.alpha = 1;
        yield return new WaitForSeconds(1);
        canVisibilityDown = true;
    }

    private void CanvasVisibility()
    {
        if (canVisibilityDown && canUpdateCG)
        {
            cg.alpha -= Time.deltaTime * visibilitySpeed;
            if(cg.alpha <= 0)
            {
                canVisibilityDown = false;
                cg.alpha = 0;
            }
        }
    }

    private void Die()
    {
        if(currentHealth <= 0)
        {
            DieEvent?.Invoke();
            DieEvent.RemoveAllListeners();
        }
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }

    private void SpawnDamageCanvas(float damageValue)
    {
        Vector3 spawnPosition = Random.insideUnitCircle + (Vector2)transform.position;
        spawnPosition.z = 0f;
        spawnPosition.y -= 1f;
        DamageController damage = Instantiate(damageController, spawnPosition, Quaternion.identity);
        damage.SetDamage(damageValue);
    }

    private void UpdateHealthText()
    {
        healthText.SetText(currentHealth.ToString());
    }

    private void UpdateHealthSlider()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = Mathf.MoveTowards(healthSlider.value, currentHealth, SpeedProgress());
    }

    private float SpeedProgress()
    {
        return progressSpeed * (maxHealth / 5) * Time.deltaTime;
    }
}
