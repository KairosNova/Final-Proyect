using UnityEngine;

public class BigGuyEnemy : EnemyBase
{
    [Header("Big Guy")]
    [SerializeField] private int damage = 50;
    private Health playerHealth;
    protected override void Awake()
    {
        base.Awake();
        health.Initialize(400);
        playerHealth = player.GetComponent<Health>();
    }
    protected override void ChasePlayer()
    {
        base.ChasePlayer();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerHealth.TakeDamage(damage);
            
        }
    }
    public override void OnDeath()
    {
        Destroy(gameObject);
    }
}
