using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockSM : StateMachine
{
    public UnityEngine.AI.NavMeshAgent agent;
    public Transform flockTransform;

    [HideInInspector]
    public Assemble assembleState;

    private void Awake()
    {
        assembleState = new Assemble(this);
    }

    protected override BaseState GetInitialState()
    {
        return assembleState;
    }

    /*protected override void OnGUI()
    {
        // this SM doesn't need to display anything
    }*/
}
