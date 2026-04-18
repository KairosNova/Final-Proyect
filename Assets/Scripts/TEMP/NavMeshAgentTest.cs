using UnityEngine;
using UnityEngine.AI;

public class NavMeshTest : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform goal;

    private void Start()
    {
        agent.updateRotation = false;
        agent.destination = goal.position;
    }
}
