using UnityEngine;

public class DogEnemy : EnemyBase, IStunnable
{
    [Header("Perro")]
    [SerializeField] private Collider2D collider;
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
        agentMovement.SetSpeed(patrolSpeed);
        if (anim != null) anim.SetFloat("Speed", patrolSpeed);
    }

    // Update is called once per frame
    protected override void ChasePlayer()
    {
        base.ChasePlayer();
        agentMovement.SetSpeed(moveSpeed);
        if (anim != null) anim.SetFloat("Speed", moveSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collider.enabled = false;
            playerHealth.TakeDamage(damage);
            agentMovement.SetState(AgentState.Static);
            if (anim != null) anim.SetTrigger("Attack");
            Destroy(gameObject, 1f);
        }
    }
    public override void OnDeath()
    {
        if (anim != null) anim.SetTrigger("Die");

        collider.enabled = false;
        Destroy(gameObject, 1);
    }

    public void OnStun()
    {
        anim.SetBool("IsStunned", true);
        agentMovement.OnStun();

        Invoke(nameof(DeStun), agentMovement.stunTime);
    }

    public void DeStun()
    {
        anim.SetBool("IsStunned", false);
        agentMovement.DeStun();
    }
}
