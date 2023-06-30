using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavSM : StateMachine
{
    public Transform target;
    public UnityEngine.AI.NavMeshAgent agent;
    public Light myLight;

    [HideInInspector]
    public Hunt huntState;
    [HideInInspector]
    public Wander wanderState;
    [HideInInspector]
    public Seek seekState;

    // used in states
    [HideInInspector]
    public Transform myTransform;
    [HideInInspector]
    public Renderer myRenderer;

    private void Awake()
    {
        huntState = new Hunt(this);
        wanderState = new Wander(this);
        seekState = new Seek(this);

        myTransform = agent.gameObject.transform;
        myRenderer = agent.gameObject.GetComponent<Renderer>();
    }

    protected override BaseState GetInitialState()
    {
        return huntState;
    }

    protected override void OnGUI()
    {
        // this SM doesn't need to display anything
    }
}
