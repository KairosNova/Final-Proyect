using System;
using UnityEngine;

public abstract class BaseState<EState> where EState : Enum
{
    public BaseState(EState key)
    {
        stateKey = key;
    }

    public EState stateKey { get; private set; }
    public abstract void Start();
    public abstract void Update();

    public abstract void OnEnterState();
    public abstract void OnExitState();

    public abstract void OnTriggerEnter2D(Collider2D other);
    public abstract void OnTriggerStay2D(Collider2D other);
    public abstract void OnTriggerExit2D(Collider2D other);
}
