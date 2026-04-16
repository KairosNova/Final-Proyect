using UnityEngine;

public class DogEnemy : EnemyBase
{
    [Header("Perro")]
    [SerializeField] private int damage = 50;
    private Health playerHealth;
    protected override void Awake()
    {
        base.Awake();
        health.Initialize(100);
        playerHealth = player.GetComponent<Health>();
    }

    // Update is called once per frame
    protected override void ChasePlayer()
    {
        base.ChasePlayer();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerHealth.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
    public override void OnDeath()
    {
        Destroy(gameObject);
    }
}
