using UnityEngine;
using UnityEngine.AI;

public enum AgentState
{
    IdlingAround, // Moviendose a puntos aleatorios sin irse muy lejos
    Static, // Quieto
    FollowingTarget // Siguiendo al objetivo
}

public class BasicAIStateMachine : MonoBehaviour, IStunnable
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
    //[SerializeField] private Transform decoytarget;
    private Transform target;

    [Header("STUN")]
    [SerializeField] private float stunTime;

    private void Start()
    {
        agent.updateRotation = false;
        target = GameObject.FindWithTag("Player").transform;

        NavMeshHit navMeshHit;
        if (NavMesh.SamplePosition(transform.position, out navMeshHit, 100, -1))
        {
            transform.position = navMeshHit.position;
        }

        //Invoke(nameof(StartCurrentState), 2f);

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
        Debug.Log("Estado Iniciado");
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
        NavMeshHit navMeshHit;
        if (NavMesh.SamplePosition(target.position, out navMeshHit, 100, -1))
        {
            agent.SetDestination(navMeshHit.position);
        }
    }

    public void SetTarget(Transform target)
    {
        if (target == this.target) return;
        this.target = target;
    }

    public void SetSpeed(float speed)
    {
        agent.speed = speed;
    }

    public void OnStun()
    {
        Debug.Log("Stuneado");
        agent.isStopped = true;
        Invoke(nameof(DeStun), stunTime);
    }
    public void DeStun()
    {
        Debug.Log("DESStuneado");
        agent.isStopped = false;
    }
}
