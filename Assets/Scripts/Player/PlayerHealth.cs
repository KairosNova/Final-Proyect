using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health
{
    [Header("Datos Persistentes")]
    public PlayerData playerData;
    private Animator anim;
    public static event Action<float> onHealthPercentageChanged;
    public void Start()
    {
        anim = GetComponentInChildren<Animator>();
         Debug.Log($"Animator encontrado: {anim.gameObject.name}");
    }

    public void OnEnable()
    {
        if (playerData != null)
        {
            this.maxHealth = playerData.maxHealth;
            this.currentHealth = playerData.currentHealth;
            // Actualizamos la UI inmediatamente al cargar la escena
            onHealthPercentageChanged?.Invoke(this.currentHealth / this.maxHealth);
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (playerData != null)
        {
            playerData.currentHealth = this.currentHealth;
        }
        
        onHealthPercentageChanged?.Invoke(base.CurrentHealth / base.MaxHealth);
        if (currentHealth <= 0)
        {
            OnDeath();
            StartCoroutine(DelayedEvent());
        }
        
    }
    public override void OnDeath()
    {
        if (anim != null)anim.SetTrigger("Die");
    }
    private IEnumerator DelayedEvent()
    {
        yield return new WaitForSeconds(0.8f);
        gameObject.SetActive(false);
    }
}
