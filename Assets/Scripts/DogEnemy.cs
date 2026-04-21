using UnityEngine;

public class DogEnemy : EnemyBase
{
    [Header("Perro")]
    [SerializeField] private int damage = 50;
    private Animator anim;
    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }
     protected override void Patrol()
    {
        base.Patrol();
        if (anim != null) anim.SetFloat("Speed", patrolSpeed);
    }

    // Update is called once per frame
    protected override void ChasePlayer()
    {
        base.ChasePlayer();
        if (anim != null) anim.SetFloat("Speed", moveSpeed);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerHealth.TakeDamage(damage);
            if (anim != null) anim.SetTrigger("Attack");
            Destroy(gameObject, 1f);
        }
    }
    public override void OnDeath()
    {
        if (anim != null) anim.SetTrigger("Die");
        Destroy(gameObject);
    }
}
