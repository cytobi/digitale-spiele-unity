using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavSM : StateMachine
{
    public Transform target;
    public UnityEngine.AI.NavMeshAgent agent;

    [HideInInspector]
    public Hunt huntState;

    private void Awake()
    {
        huntState = new Hunt(this);
    }

    protected override BaseState GetInitialState()
    {
        return huntState;
    }
}
