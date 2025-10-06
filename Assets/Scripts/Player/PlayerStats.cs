using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private HealthbarControl healthbarControl;
    [SerializeField] private float maxHealth;
    private float currentHealth;
    [Header("Flash")]
    [SerializeField] private float flashDuration;
    [SerializeField, Range(0, 1)] private float flashStrength;
    [SerializeField] private Color flashCol;
    [SerializeField] private Material flashMaterial;
    private Material defaultMaterial;
    private SpriteRenderer spriter;
    private bool canTakeDamage = true;

    [Header("StatsCollider")]
    [SerializeField] private Collider2D standingStatsCol;
    [SerializeField] private Collider2D crouchStatsCol;
    private Collider2D currentStatsCol;
    void Start()
    {
        currentHealth = maxHealth;
        healthbarControl.SetSliderValue(currentHealth, maxHealth);
        spriter = GetComponentInParent<SpriteRenderer>();
        defaultMaterial = spriter.material;
        
        // Otomatik referans bulma
        if (flashMaterial == null)
        {
            flashMaterial = Resources.Load<Material>("FlashMaterial");
            if (flashMaterial == null)
            {
                // Materials klasöründe ara
                flashMaterial = Resources.Load<Material>("Materials/FlashMaterial");
            }
        }
        
        if (player == null)
        {
            player = GetComponentInParent<Player>();
        }
        
        if (healthbarControl == null)
        {
            healthbarControl = FindObjectOfType<HealthbarControl>();
        }
    }

    public void DamagePlayer(float damage)
    {
        if (canTakeDamage == false)
            return;
        currentHealth -= damage;

        healthbarControl.SetSliderValue(currentHealth, maxHealth);
        StartCoroutine(Flash());
        if (currentHealth <= 0)
        {
            DisableStatsCollider();
            if (player.stateMachine.currentState != PlayerStates.State.Knockback)
                player.stateMachine.ChangeState(PlayerStates.State.Death);
            
            
        }
    }

    private IEnumerator Flash()
    {
        canTakeDamage = false;
        spriter.material = flashMaterial;
        flashMaterial.SetColor("_FlashColor", flashCol);
        flashMaterial.SetFloat("_FlashAmount", flashStrength);
        yield return new WaitForSeconds(flashDuration);
        spriter.material = defaultMaterial;
        if (currentHealth > 0)
            canTakeDamage = true;
    }


    public void EnableStandCol()
    {
        if (currentHealth <= 0)
            return;
        crouchStatsCol.enabled = false;
        standingStatsCol.enabled = true;
        currentStatsCol = standingStatsCol;
    }
    public void EnableCrouchCol()
    {
        if (currentHealth <= 0)
            return;
        crouchStatsCol.enabled = true;
        standingStatsCol.enabled = false;
        currentStatsCol = crouchStatsCol;
    }
    public bool GetCanTakeDamage()
    {
        return canTakeDamage;
    }

    public void DisableStatsCollider()
    {
        // Her iki collider'ı da devre dışı bırak - currentStatsCol null olabilir
        if (standingStatsCol != null)
            standingStatsCol.enabled = false;
        if (crouchStatsCol != null)
            crouchStatsCol.enabled = false;
    }

    public void EnableStatsCollider()
    {
        currentStatsCol.enabled = true;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void DisableDamage()
    {
        canTakeDamage = false;
    }
    public void EnableDamage()
    {
        canTakeDamage = true;
    }
}


