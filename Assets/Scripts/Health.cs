using NUnit.Framework;
using UnityEngine;


public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float currentHealth;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsDead => currentHealth <= 0;
    public float HealthPercent => (float)currentHealth / maxHealth;
    
    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void Initialize(int hp)
    {
        maxHealth = hp;
        currentHealth = hp;
    }

    public virtual void TakeDamage(int damage)
    {
        if(IsDead)
        {
            return;
        }
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        Debug.Log($"{gameObject.name} recibió {damage} daño. HP: {currentHealth}/{maxHealth}");
        if (IsDead)
        {
            OnDeath();
        }
    }
    
    protected virtual void OnDeath()
    {
        Debug.Log($"{gameObject.name} murió");
        GetComponent<EnemyBase>()?.OnDeath();
    }
}
