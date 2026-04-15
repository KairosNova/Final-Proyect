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
    protected bool hasDetectedPlayer = false;
    protected Vector2 moveDirection;
    [Header("Giro")]
    protected bool facingRight = true;
    protected virtual void Awake()
    {
        health = GetComponent<Health>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Update()
    {
        if (!hasDetectedPlayer)
        {
            TryDetectPlayer();
        }else
        {
            ChasePlayer();
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
        if (distanceToPlayer > detectionRange)
        {
            return;
        } 
        float angle = Vector2.Angle(moveDirection, directionToPlayer);
        if (angle <= detectionAngle / 2f)
        {
            hasDetectedPlayer = true;
            Debug.Log($"{gameObject.name} detectó al jugador");
        }

    }
    protected virtual void ChasePlayer()
    {
        if (player == null) 
        {
            return;
        }
        moveDirection = (player.position - transform.position).normalized;
        transform.position += (Vector3)moveDirection * moveSpeed * Time.deltaTime;
        if (moveDirection.x > 0 && !facingRight)
        {
            Flip();
        }else if (moveDirection.x < 0 && facingRight)
        {
            Flip();
        }

    }

    protected virtual void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
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
