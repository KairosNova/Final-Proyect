using UnityEngine;


public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float currentHealth;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsDead => currentHealth <= 0;
    public float HealthPercent => (float)currentHealth / maxHealth;
    public bool isInvulnerable = false;
    
    void Awake()
    {
        currentHealth = maxHealth;
    }
    public void SetInvulnerable(bool invulnerable)
    {
        isInvulnerable = invulnerable;
    }

    public virtual void TakeDamage(int damage)
    {
        if(IsDead)
        {
            return;
        }
        if (isInvulnerable)
        {
            return;
        }
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        Debug.Log($"{gameObject.name} recibió {damage} daño. HP: {currentHealth}/{maxHealth}");
        if (currentHealth <= 0)
        {
            Death();
        }
        
    }
    
    protected virtual void Death()
    {
        Debug.Log($"{gameObject.name} murió");
        SendMessage("OnDeath", SendMessageOptions.DontRequireReceiver);
    }
}
