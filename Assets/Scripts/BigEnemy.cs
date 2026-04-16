using UnityEngine;
using System.Collections;

public class BigEnemy : MonoBehaviour
{
    // 1. Definimos los 3 estados exactos en los que puede estar el enemigo
    public enum BossState { Chasing, Attacking, Vulnerable, Dead }
    
    [Header("Estado Actual (Solo visualización)")]
    public BossState currentState = BossState.Chasing;

    [Header("Estadísticas")]
    public float moveSpeed = 3f;
    public int damage = 50;

    [Header("Ataque y Vulnerabilidad")]
    public float attackRange = 2f;
    public float attackWindup = 0.5f; // Tiempo que "carga" el golpe antes de darlo
    public float vulnerableDuration = 3f;
    [Header("Tiempos de Animación")]
    public float deathAnimationTime = 1.2f; // Ajusta esto a lo que dure tu animación de muerte

    // Referencias internas
    private Transform player;
    private Health myHealth;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private Rigidbody2D rb;

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
        switch (currentState)
        {
            case BossState.Chasing:
                ChasePlayer();
                CheckAttackRange();
                break;

            case BossState.Attacking:
                // Se queda quieto mientras ataca (la lógica está en la Corrutina)
                break;

            case BossState.Vulnerable:
                // Se queda quieto y vulnerable (la lógica está en la Corrutina)
                break;
        }
    }

    private void ChasePlayer()
    {
        if (anim != null) anim.SetBool("isChasing", true);
        // 1. Moverse hacia el jugador (Ignoramos físicas complejas, vamos a lo seguro)
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        // 2. Mirar al jugador (Flip simple)
        if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1); // Mira a la derecha
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1); // Mira a la izquierda
        }
    }

    private void CheckAttackRange()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= attackRange)
        {
            StartCoroutine(AttackSequence());
        }
    }

    private IEnumerator AttackSequence()
    {
        // Cambiamos de estado: Deja de perseguir
        currentState = BossState.Attacking;
        if (anim != null)
        {
            anim.SetBool("isChasing", false);
            anim.SetTrigger("Attack");
        }

        // 1. Telegrafiar el golpe (se pone naranja y espera medio segundo)
        spriteRenderer.color = new Color(1f, 0.5f, 0f); 
        yield return new WaitForSeconds(attackWindup);

        // 2. Dar el golpe (Detección por círculo invisible)
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Health playerHealth = hit.GetComponent<Health>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                    Debug.Log("¡El Big Guy te aplastó!");
                }
            }
        }

        // 3. Inmediatamente después de pegar, se vuelve vulnerable
        StartCoroutine(VulnerableSequence());
    }

    private IEnumerator VulnerableSequence()
    {
        // Cambiamos de estado
        currentState = BossState.Vulnerable;
        if (anim != null) anim.SetTrigger("Vulnerable");
        
        // Se pone rojo y SE APAGA LA INVULNERABILIDAD
        spriteRenderer.color = Color.red;
        if (myHealth != null) myHealth.SetInvulnerable(false);
        Debug.Log("¡DALE AHORA! Es vulnerable.");

        // Espera el tiempo de vulnerabilidad
        yield return new WaitForSeconds(vulnerableDuration);

        // Se recupera: Vuelve a ser blanco, inmortal y persigue
        spriteRenderer.color = Color.white;
        if (myHealth != null) myHealth.SetInvulnerable(true);
        currentState = BossState.Chasing;
    }
    public void OnDeath()
    {
        Debug.Log("¡MENSAJE RECIBIDO! BigGuy empezando secuencia de muerte.");
        if (currentState == BossState.Dead) return;
        StartCoroutine(DeathRoutine());
    }
    private IEnumerator DeathRoutine()
    {
        currentState = BossState.Dead;
        if (anim != null) anim.SetTrigger("Die");
        if (rb != null) rb.linearVelocity = Vector2.zero;
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(deathAnimationTime);
        Destroy(gameObject);
    }

    // Dibuja el círculo de ataque en Unity para que puedas medirlo bien
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}