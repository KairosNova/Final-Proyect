using System;
using UnityEngine;

public class PlayerHealth : Health
{
    [Header("Datos Persistentes")]
    public PlayerData playerData;
    public static event Action<float> onHealthPercentageChanged;
    public void Start()
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
    }
}
