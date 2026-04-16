using UnityEngine;

public class DogEnemy : EnemyBase
{
    [Header("Perro")]
    [SerializeField] private int damage = 50;

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
