using UnityEngine;
using System.Collections;

public class BigGuyEnemy : EnemyBase
{
    [Header("Big Guy")]
    [SerializeField] private int damage = 50;
    [SerializeField] private float vulnerableWindow = 2f;
    [SerializeField] private float attackDuration = 0.5f;

    [Header("Ataque Automático")]
    [SerializeField] private float attackCooldown = 2f;   // tiempo entre ataques
    [SerializeField] private float attackRange = 1.5f;    // distancia para atacar
    private float currentCooldown = 0f;
    private SpriteRenderer spriteRenderer;
    private bool isAttackingOrVulnerable = false;
    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health.SetInvulnerable(true);
        spriteRenderer.color = Color.white;

    }
    protected override void Update()
    {
        if (isAttackingOrVulnerable) return;
        base.Update();
        if (!hasDetectedPlayer) return;
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
        if ( currentCooldown <= 0f)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer <= attackRange)
            {
                PerformAttack();
            }
        }
    }
    public void PerformAttack()
    {
        
        StartCoroutine(AttackRoutine());
    }
    private IEnumerator AttackRoutine()
    {
        isAttackingOrVulnerable = true;
        currentCooldown = attackCooldown;
        Debug.Log("BigGuy prepara el golpe...");
        yield return new WaitForSeconds(attackDuration);
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (Collider2D hit in hitObjects)
        {
            if (hit.CompareTag("Player"))
            {
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                    Debug.Log("BigGuy golpeó al jugador exitosamente");
                }
            }
            
        }
        StartCoroutine(VulnerableWindow());
    }

    private IEnumerator VulnerableWindow()
    {
        health.SetInvulnerable(false);
        spriteRenderer.color = Color.red;
        Debug.Log($"{gameObject.name} ahora es VULNERABLE por {vulnerableWindow} segundos");
        yield return new WaitForSeconds(vulnerableWindow);
        health.SetInvulnerable(true);
        spriteRenderer.color = Color.white;
        Debug.Log($"{gameObject.name} recuperó el escudo");
        isAttackingOrVulnerable = false;

    }
    public override void OnDeath()
    {
        Destroy(gameObject);
    }
}
