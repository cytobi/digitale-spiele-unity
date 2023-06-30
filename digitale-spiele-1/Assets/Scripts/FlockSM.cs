using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockSM : StateMachine
{
    public UnityEngine.AI.NavMeshAgent agent;
    public Transform flockTransform;
    public Transform myTransform; // clean up by getting these from the agent reference: agent.gameObject
    public Rigidbody myRigidbody;

    [HideInInspector]
    public Assemble assembleState;

    // used in multiple states
    [HideInInspector]
    public List<Transform> flockTransformList;

    private void Awake()
    {
        assembleState = new Assemble(this);
        flockTransformList = new List<Transform>();
        for (int i = 0; i < flockTransform.childCount; i++) {
            flockTransformList.Add(flockTransform.GetChild(i));
        }
    }

    protected override BaseState GetInitialState()
    {
        return assembleState;
    }

    /*protected override void OnGUI()
    {
        // this SM doesn't need to display anything
    }*/

    public Vector3 flockCenter() {
        // calculate center of flock
        Vector3 center = Vector3.zero;
        foreach (Transform t in flockTransformList) {
            center += t.position;
        }
        center /= flockTransform.childCount;
        return center;
    }
}
