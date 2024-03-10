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
    private float hpRegen;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private float progressSpeed = 4.2f;

    [Header("Damage")]
    [SerializeField] private FloatNumber floatNumber;
    [SerializeField] private GameObject particleDamage;
    [SerializeField] private AudioSource damageAudio;
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
        StartCoroutine(RegenerateHealthRoutine());
    }

    private void Update()
    {
        UpdateHealthSlider();
        CanvasVisibility();

        if (Input.GetKeyDown(KeyCode.F))
            Damage(-1);
        if (Input.GetKeyDown(KeyCode.G))
            Damage(10);
    }

    public void SetMaxHealth(float newValue)
    {
        maxHealth = newValue;
        currentHealth = maxHealth;
        UpdateHealthText();
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void ResetCurrentHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthText();
    }

    public void Damage(float damage)
    {
        if (currentHealth > 0)
        {
            damageAudio.Play();
            currentHealth -= damage;
            SpawnDamageFloatNumber(damage);
            SpawnHitParticles();
            StartCoroutine(nameof(DamageVisibility));
            DamageEvent?.Invoke();
        }

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
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            DieEvent?.Invoke();
            DieEvent.RemoveAllListeners();
        }
    }

    private void SpawnDamageFloatNumber(float damageValue)
    {
        Transform parentTransform = transform.parent;
        Vector3 spawnPosition = (Random.insideUnitCircle * 0.7f) + (Vector2)parentTransform.position;
        spawnPosition.z = 0f;
        spawnPosition.y += 1f;
        FloatNumber floatNum = Instantiate(floatNumber, spawnPosition, Quaternion.identity);
        floatNum.InitiateFloatNumber(damageValue, 0);
    }

    private void SpawnHealFloatNumber()
    {
        Transform parentTransform = transform.parent;
        Vector3 spawnPosition = (Random.insideUnitCircle * 0.7f) + (Vector2)parentTransform.position;
        spawnPosition.z = 0f;
        spawnPosition.y += 1f;
        FloatNumber floatNum = Instantiate(floatNumber, spawnPosition, Quaternion.identity);
        floatNum.InitiateFloatNumber(hpRegen, 1);
    }

    public void SpawnHitParticles()
    {
        Transform parent = transform.parent;
        Vector3 spawnPos = parent.position;
        spawnPos.y += 1f;
        Instantiate(particleDamage, spawnPos, Quaternion.identity);
    }

    private void UpdateHealthText()
    {
        healthText.SetText(currentHealth.ToString("F2"));
    }

    private void UpdateHealthSlider()
    {
        if (currentHealth < 0) return;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = Mathf.MoveTowards(healthSlider.value, currentHealth, SpeedProgress());
    }

    private float SpeedProgress()
    {
        return progressSpeed * (maxHealth / 5) * Time.deltaTime;
    }

    public void UpdateRegenValue(float regenValue)
    {
        hpRegen = regenValue;
    }

    private IEnumerator RegenerateHealthRoutine()
    {
        while (true)
        {
            if (currentHealth < maxHealth && hpRegen > 0) 
            {
                if (GetComponentInParent<PlayerManager>().playerState == PlayerState.Die) break;
                currentHealth += hpRegen;
                SpawnHealFloatNumber();      
            }

            if (currentHealth >= maxHealth)
                currentHealth = maxHealth;

            UpdateHealthText();
            yield return new WaitForSeconds(5f);
        }
    }
}
