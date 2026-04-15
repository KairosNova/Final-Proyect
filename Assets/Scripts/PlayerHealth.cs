using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Scrollbar healthBar;
    
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.size =1f;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        float healthPercent = (float)currentHealth / maxHealth;
        healthBar.size = healthPercent;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
