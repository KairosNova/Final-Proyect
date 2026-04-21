using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("Detección")]
    [SerializeField] protected float detectionRange = 5f;
    [SerializeField] protected float detectionAngle = 60f;

    [Header("Movimiento")]
    [SerializeField] protected float moveSpeed = 3f;
    protected Transform player;
    protected Health health;
    protected Health playerHealth;
    protected bool hasDetectedPlayer = false;
    protected Vector2 moveDirection;

    [SerializeField] protected BasicAIStateMachine agentMovement;

    [Header("Giro")]
    protected bool facingRight = true;
    protected Rigidbody2D rb;

    [Header("Patrullaje (Waypoints)")]
    public Transform[] waypoints; // Puntos por los que rondará
    public float patrolSpeed = 2f; // Velocidad al rondar (suele ser más lenta que perseguir)
    private int currentWaypointIndex = 0;

    protected virtual void Awake()
    {
        health = GetComponent<Health>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        if (!hasDetectedPlayer)
        {
            Patrol();
            TryDetectPlayer();
        }else
        {
            ChasePlayer();
        }
    }

    protected virtual void Patrol()
    {
        Debug.Log("Patrullando");
        if (waypoints == null || waypoints.Length == 0) return;

        Transform target = waypoints[currentWaypointIndex];
        //transform.position = Vector2.MoveTowards(transform.position, target.position, patrolSpeed * Time.deltaTime);
        
        agentMovement.SetTarget(target);
        agentMovement.SetState(AgentState.FollowingTarget);

        Vector2 direction = target.position - transform.position;
        if (direction.x > 0.01f && !facingRight) Flip();
        else if (direction.x < -0.01f && facingRight) Flip();
        // Si ya llegó al punto, cambiamos al siguiente objetivo de la lista
        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            currentWaypointIndex++;
            // Si llegó al último punto, vuelve a empezar desde el 0 (creando un bucle infinito)
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }
    }
    private void TryDetectPlayer()
    {
        if (player == null)
        { 
            return;
        }

        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > detectionRange) return;

        Vector2 currentDirection = moveDirection == Vector2.zero ? Vector2.right : moveDirection;

        float angle = Vector2.Angle(currentDirection, directionToPlayer);

        if (angle <= detectionAngle / 2f)
        {
            hasDetectedPlayer = true;
            agentMovement.SetTarget(player);
            Debug.Log($"{gameObject.name} detectó al jugador");
        } 
    }

    protected virtual void ChasePlayer()
    {
        if (player == null) 
        {
            return;
        }
        float distance = Vector2.Distance(transform.position, player.position);
        moveDirection = (player.position - transform.position).normalized;
        if (distance > 0.5f)
        {
            //rb.linearVelocity = moveDirection * moveSpeed;
            agentMovement.SetState(AgentState.FollowingTarget);
        }
        if (Mathf.Abs(moveDirection.x) > 0.1f)
        {
            if (moveDirection.x > 0 && !facingRight) Flip();
            else if(moveDirection.x < 0 && facingRight)
            { 
                Flip();
            }
        }
    }

    protected virtual void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    
    protected void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
    }
    
    public virtual void OnDeath()
    {
        Destroy(gameObject);
    }

    protected virtual void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            moveDirection = Vector2.right; 
        }
        Gizmos.color = hasDetectedPlayer ? Color.red : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        float halfAngle = detectionAngle / 2f * Mathf.Deg2Rad;
        Vector2 leftEdge = new Vector2(moveDirection.x * Mathf.Cos(halfAngle) - moveDirection.y * Mathf.Sin(halfAngle),moveDirection.x * Mathf.Sin(halfAngle) + moveDirection.y * Mathf.Cos(halfAngle));
        Vector2 rightEdge = new Vector2(moveDirection.x * Mathf.Cos(-halfAngle) - moveDirection.y * Mathf.Sin(-halfAngle),moveDirection.x * Mathf.Sin(-halfAngle) + moveDirection.y * Mathf.Cos(-halfAngle));
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, (Vector3)leftEdge * detectionRange);
        Gizmos.DrawRay(transform.position, (Vector3)rightEdge * detectionRange);
    }
}
