using System;
using System.Collections.Generic;
using UnityEngine;

public class StateManager<StatesEnum> : MonoBehaviour where StatesEnum : Enum
{
    protected Dictionary<StatesEnum, BaseState<StatesEnum>> states = new Dictionary<StatesEnum, BaseState<StatesEnum>>();
}
