using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Animator animator;
    public float moveSpeed = 2f;
    public float restTime = 5f;

    private float restTimer;
    private bool isAttacking = false;

    void Update()
    {
        if (!isAttacking)
        {
            Walk();
        }
    }

    void Walk()
    {
        // Simple movement (example)
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

        animator.Play("Walk");
    }

    // Call this when enemy detects player (trigger, distance, etc.)
    public void StartAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            StartCoroutine(AttackRoutine());
        }
    }

    System.Collections.IEnumerator AttackRoutine()
    {
        // 🥊 Attack 1
        animator.SetTrigger("BigGuy_Attack1");
        yield return new WaitForSeconds(1f); // match animation length

        // 🥊 Attack 2
        animator.SetTrigger("BigGuy_Attack2");
        yield return new WaitForSeconds(1f);

        // 😴 Rest
        animator.SetTrigger("Rest");
        yield return new WaitForSeconds(restTime);

        isAttacking = false;
    }
}