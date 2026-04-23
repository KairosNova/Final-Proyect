using System;
using UnityEngine;

public abstract class Transition : MonoBehaviour
{
    public static event Action OnStartTransitionFinished;

    public abstract void StartTransition();
    public abstract void EndTransition();
}
