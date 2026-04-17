using UnityEngine;
using UnityEngine.AI;

public enum AgentState
{
    IdlingAround, // Moviendose a puntos aleatorios sin irse muy lejos
    Static, // Quieto
    FollowingTarget // Siguiendo al objetivo
}

public class AgentMovement : MonoBehaviour
{
    [SerializeField] private AgentState state;
    [SerializeField] private NavMeshAgent agent;

    [Header("IDLE")]
    [SerializeField] private float idlingRadius;
    [SerializeField] private float idleMinChangePositionTime;
    [SerializeField] private float idleMaxChangePositionTime;
    private Vector3 idleOriginPoint;

    [Header("TARGET FOLLOWING")]
    [SerializeField] private float followingCheckRate;
    private Transform target;
    private Vector3 homePosition;

    private void Start()
    {
        agent.updateRotation = false;
        StartCurrentState();
    }

    public void SetState(AgentState state)
    {
        if (this.state == state) return;

        EndCurrentState();

        this.state = state;

        StartCurrentState();
    }

    private void StartCurrentState()
    {
        switch (state)
        {
            case AgentState.Static:
                agent.isStopped = true;
            break;

            case AgentState.IdlingAround:
                idleOriginPoint = transform.position;
                IdleAround();
            break;

            case AgentState.FollowingTarget:
                InvokeRepeating(nameof(FollowTarget), 0f, followingCheckRate);
            break;
        }
    }

    private void EndCurrentState()
    {
        switch (state)
        {
            case AgentState.Static:
                agent.isStopped = false;
            break;

            case AgentState.IdlingAround:
                CancelInvoke(nameof(IdleAround));
            break;

            case AgentState.FollowingTarget:
                CancelInvoke(nameof(FollowTarget));
            break;
        }
    }

    private void IdleAround()
    {
        Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

        agent.SetDestination(idleOriginPoint + (Vector3)randomDirection * Random.Range(0f, idlingRadius));
        Invoke(nameof(IdleAround), Random.Range(idleMinChangePositionTime, idleMaxChangePositionTime));
    }

    private void FollowTarget()
    {
        agent.SetDestination(target.position);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
