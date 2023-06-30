using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockSM : StateMachine
{
    public UnityEngine.AI.NavMeshAgent agent;
    public Transform flockTransform;
    public Transform huntersTransform;

    [HideInInspector]
    public Assemble assembleState;
    [HideInInspector]
    public RunAway runAwayState;

    // used in states
    [HideInInspector]
    public List<Transform> flockTransformList;
    [HideInInspector]
    public List<Transform> huntersTransformList;
    [HideInInspector]
    public float wantedDistance = 3f;
    [HideInInspector]
    public Transform myTransform;
    [HideInInspector]
    public Rigidbody myRigidbody;
    [HideInInspector]
    public Renderer myRenderer;

    private void Awake()
    {
        assembleState = new Assemble(this);
        runAwayState = new RunAway(this);

        flockTransformList = new List<Transform>();
        for (int i = 0; i < flockTransform.childCount; i++) {
            flockTransformList.Add(flockTransform.GetChild(i));
        }

        huntersTransformList = new List<Transform>();
        for (int i = 0; i < huntersTransform.childCount; i++) {
            huntersTransformList.Add(huntersTransform.GetChild(i));
        }

        myTransform = agent.gameObject.transform;
        myRigidbody = agent.gameObject.GetComponent<Rigidbody>();
        myRenderer = agent.gameObject.GetComponent<Renderer>();
    }

    protected override BaseState GetInitialState()
    {
        return assembleState;
    }

    protected override void OnGUI()
    {
        // this SM doesn't need to display anything
    }

    public Vector3 flockCenter() {
        // calculate center of flock
        Vector3 center = Vector3.zero;
        foreach (Transform t in flockTransformList) {
            center += t.position;
        }
        center /= flockTransform.childCount;
        return center;
    }

    public Transform closestHunter() {
        Vector3 flockCenter = this.flockCenter();
        Transform closestHunter = huntersTransformList[0];
        float closestDistance = Vector3.Distance(flockCenter, closestHunter.position);
        foreach (Transform t in huntersTransformList) {
            float distance = Vector3.Distance(flockCenter, t.position);
            if (distance < closestDistance) {
                closestDistance = distance;
                closestHunter = t;
            }
        }
        return closestHunter;
    }

    public float closestHunterDistance() {
        return Vector3.Distance(myTransform.position, closestHunter().position);
    }
}
