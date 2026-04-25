using UnityEngine;
using System.Collections;

public class BigEnemy : MonoBehaviour
{
    // 1. Definimos los 3 estados exactos en los que puede estar el enemigo
    public enum BossState { Chasing, Attacking, Vulnerable, Dead }
    
    [Header("Estado Actual (Solo visualización)")]
    public BossState currentState = BossState.Chasing;
    [Header("Detección")]
    public float detectionRange = 5f; // Rango para que empiece a perseguir
    public bool hasDetectedPlayer = false;

    [Header("Estadísticas")]
    public float moveSpeed = 3f;
    public int damage = 50;
    public int damageShield = 20;

    [Header("Ataque y Vulnerabilidad")]
    public float attackRange = 2f;// rango ataque 1
    public float shieldRange = 1.2f;    // rango ataque 2 — muy cerca
  
    
    [Header("Tiempos")]
    public float attackWindup = 0.5f; // Tiempo que "carga" el golpe antes de darlo
    public float vulnerableDuration = 3f;
    public float deathAnimationTime = 0.6f; // Ajusta esto a lo que dure tu animación de muerte
    public float attackCooldown = 2f;
    private float currentCooldown = 0f;

    // Referencias internas
    private bool isAttacking = false; 
    private Transform player;
    private Health myHealth;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private Rigidbody2D rb;
    private int attackCount = 0;

    void Start()
    {
        // Buscamos todo automáticamente para evitar errores de referencias nulas
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        myHealth = GetComponent<Health>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Inicia el juego siendo INVULNERABLE
        if (myHealth != null) myHealth.SetInvulnerable(true);
    }

    void Update()
    {
        if (currentState == BossState.Dead || player == null) return;

        // La Máquina de Estados: El enemigo decide qué hacer según su estado
        if (currentCooldown > 0)currentCooldown -= Time.deltaTime;
        float distance = Vector2.Distance(transform.position, player.position);
        float distanceX = Mathf.Abs(player.position.x - transform.position.x);
        
        if (!hasDetectedPlayer && distance <= detectionRange)
        {
            hasDetectedPlayer = true;
            Debug.Log("¡El Big Guy te ha visto!");
        }

        switch (currentState)
        {
            case BossState.Chasing:            
                
                if (hasDetectedPlayer)
                {
                    ChasePlayer(distance);
                    
                    CheckAttackRange(distanceX);
                }else
                {
                    // Si no te ha visto, se asegura de estar en Idle
                    if (anim != null) anim.SetFloat("Speed", 0f);
                }
                
                break;

            case BossState.Attacking:
                break;

            case BossState.Vulnerable:
                // Se queda quieto y vulnerable (la lógica está en la Corrutina)
                if (anim != null) anim.SetFloat("Speed", 0f);// Quieto en estos estados
                break;
        }
    }

    private void ChasePlayer(float distance)
    {
        // Solo se mueve si no está pegado al jugador
        if (distance > 0.5f)
        {
            transform.position = Vector2.MoveTowards(transform.position,player.position, moveSpeed * Time.deltaTime);
            if (anim != null) anim.SetFloat("Speed", moveSpeed); // Enviamos la velocidad al Animator
        }else
        {
            if (anim != null) anim.SetFloat("Speed", 0f);
        }
        
        // Mirar al jugador (Flip simple)
        if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1); // Mira a la derecha
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1); // Mira a la izquierda
        }
    }

    private void CheckAttackRange(float distance)
    {
        if (currentState != BossState.Chasing) return;
        if (isAttacking) return;  
        if (currentCooldown > 0) return;
        if (attackCount > 0) return;
        
        // Si está muy cerca → ataque 2 escudo, no deja vulnerable
        if (distance <= shieldRange)
        {
            isAttacking = true;
            StartCoroutine(ShieldAttackSequence());
        }else if (distance <= attackRange)
        {
            isAttacking = true;
            StartCoroutine(AttackSequence());
        }
    }
    // ATAQUE 1 — fuerte, deja vulnerable
    private IEnumerator AttackSequence()
    {
        
        // Cambiamos de estado: Deja de perseguir
        currentState = BossState.Attacking;
        currentCooldown = attackCooldown;
        if (anim != null)
        {
            anim.SetFloat("Speed", 0f);
            anim.SetTrigger("Attack");
        }

        // 1. Telegrafiar el golpe (se pone naranja y espera medio segundo)
        spriteRenderer.color = new Color(1f, 0.5f, 0f);
        yield return new WaitForSeconds(attackWindup);

        // 2. Dar el golpe (Detección por círculo invisible)
        spriteRenderer.color = Color.white;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);
        bool alreadyHit = false;
        foreach (Collider2D hit in hits)
        {
            if (alreadyHit) break;
            if (!hit.CompareTag("Player")) continue;
            
            Health playerHealth = hit.transform.root.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("¡El Big Guy te aplastó!");
                alreadyHit = true;
            }
            
        }

        // 3. Inmediatamente después de pegar, se vuelve vulnerable
        StartCoroutine(VulnerableSequence());
    }
     // ATAQUE 2 — escudo, NO deja vulnerable
    private IEnumerator ShieldAttackSequence()
    {
        currentState = BossState.Attacking;
        currentCooldown = attackCooldown / 2f; // cooldown más corto
        if (anim != null)
        {
            anim.SetFloat("Speed", 0f);
            anim.SetTrigger("Attack2");
        }
        spriteRenderer.color = new Color(0.5f, 0.5f, 1f); // azul — telegrafía escudo
        yield return new WaitForSeconds(attackWindup / 2f);
        // Golpe escudo
        spriteRenderer.color = Color.white;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, shieldRange + 0.3f);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Health playerHealth = hit.GetComponent<Health>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageShield);
                    break;
                }
            }
        }
        yield return new WaitForSeconds(0.5f);// Pequeña pausa para recuperar postura
        currentState = BossState.Chasing;
        isAttacking = false;
        attackCount = 0;
    }


    private IEnumerator VulnerableSequence()
    {
        // Cambiamos de estado
        currentState = BossState.Vulnerable;
        // Sin trigger — la transición sale sola por Exit Time de Attack1
        // Se pone rojo y SE APAGA LA INVULNERABILIDAD
        spriteRenderer.color = Color.red;
        if (myHealth != null) myHealth.SetInvulnerable(false);

        // Espera el tiempo de vulnerabilidad
        yield return new WaitForSeconds(vulnerableDuration);

        // Se recupera: Vuelve a ser blanco, inmortal y persigue
        spriteRenderer.color = Color.white;
        if (myHealth != null) myHealth.SetInvulnerable(true);
        currentState = BossState.Chasing;
        isAttacking = false;
    }
    public void OnDeath()
    {
        if (currentState == BossState.Dead) return;
        StartCoroutine(DeathRoutine());
    }
    private IEnumerator DeathRoutine()
    {
        currentState = BossState.Dead;
        if (anim != null) anim.SetTrigger("Die");
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(deathAnimationTime);
        Destroy(gameObject);
    }

    // Dibuja el círculo de ataque en Unity para que puedas medirlo bien
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, shieldRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}