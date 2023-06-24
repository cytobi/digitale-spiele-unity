using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavSM : StateMachine
{
    public Transform target;
    public UnityEngine.AI.NavMeshAgent agent;
    public Transform myTransform;

    [HideInInspector]
    public Hunt huntState;
    [HideInInspector]
    public Wander wanderState;
    [HideInInspector]
    public Seek seekState;

    private void Awake()
    {
        huntState = new Hunt(this);
        wanderState = new Wander(this);
        seekState = new Seek(this);
    }

    protected override BaseState GetInitialState()
    {
        return huntState;
    }
}
