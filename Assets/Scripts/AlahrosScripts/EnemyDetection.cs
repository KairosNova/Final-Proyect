using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10f;

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < detectionRange)
        {
            Debug.Log("Player detected!");
            // 👉 Call attack or chase here
        }
    }
}