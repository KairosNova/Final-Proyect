using System;
using UnityEngine;

public class PlayerHealth : Health
{
    public static event Action<float> onHealthPercentageChanged;

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        onHealthPercentageChanged?.Invoke(base.CurrentHealth / base.MaxHealth);
    }
}
