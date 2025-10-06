using System.Collections;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    
    [SerializeField] protected float health;
    [SerializeField] protected float maxHealth;

    [Header("Flash")]
    [SerializeField] private float flashDuration;
    [SerializeField, Range(0, 1)] private float flashStrength;
    [SerializeField] private Color flashCol;
    [SerializeField] private Material flashMaterial;
    private Material defaultMaterial;
    [SerializeField] private SpriteRenderer spriter;


    protected Coroutine damageCoroutine;
    private Material flastMatInstance;



    private void Start()
    {
        defaultMaterial = spriter.material;
        flastMatInstance = new Material(flashMaterial);
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        DamageProcess();
        if (damageCoroutine != null)
            StopCoroutine(damageCoroutine);

        damageCoroutine = StartCoroutine(Flash());
        if (health <= 0)
        {
            DeathProcess();
        }
    }



    protected virtual void DamageProcess()
    {
        // customize in a child class
    }

    protected virtual void DeathProcess()
    {
        // customize in a child class
    }

    private IEnumerator Flash()
    {

        flastMatInstance.SetTexture("_MainTex", defaultMaterial.mainTexture);
        spriter.material = flastMatInstance;
        flastMatInstance.SetColor("_FlashColor", flashCol);
        flastMatInstance.SetFloat("_FlashAmount", flashStrength);
        yield return new WaitForSeconds(flashDuration);
        spriter.material = defaultMaterial;
        damageCoroutine = null;
    }
}
