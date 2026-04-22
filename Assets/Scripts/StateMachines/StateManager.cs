using System;
using System.Collections.Generic;
using UnityEngine;

public class StateManager<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> states = new Dictionary<EState, BaseState<EState>>();

    public BaseState<EState> currentState;

    private bool isTransitioning = false;

    protected void Start()
    {
        currentState.Start();
    }

    protected void Update()
    {
        if (isTransitioning) return;
        currentState.Update();
    }

    protected void TransitionToState(EState stateKey)
    {
        isTransitioning = true;
        currentState.OnExitState();
        currentState = states[stateKey];
        currentState.OnEnterState();
        isTransitioning = false;
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (isTransitioning) return;
        currentState.OnTriggerEnter2D(other);
    }

    protected void OnTriggerStay2D(Collider2D other)
    {
        if (isTransitioning) return;
        currentState.OnTriggerStay2D(other);
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (isTransitioning) return;
        currentState.OnTriggerExit2D(other);
    }
}