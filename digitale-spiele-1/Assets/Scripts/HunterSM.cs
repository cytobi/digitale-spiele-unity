using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterSM : StateMachine
{
    public Transform flockTransform;
    public Transform huntersTransform;
    public UnityEngine.AI.NavMeshAgent agent;

    [HideInInspector]
    public FlockingHunt huntState;
    [HideInInspector]
    public FlockingWander wanderState;

    // used in states
    [HideInInspector]
    public List<Transform> flockTransformList;
    [HideInInspector]
    public Transform myTransform;
    [HideInInspector]
    public Renderer myRenderer;

    private void Awake()
    {
        huntState = new FlockingHunt(this);
        wanderState = new FlockingWander(this);

        flockTransformList = new List<Transform>();
        for (int i = 0; i < flockTransform.childCount; i++) {
            flockTransformList.Add(flockTransform.GetChild(i));
        }

        myTransform = agent.gameObject.transform;
        myRenderer = agent.gameObject.GetComponent<Renderer>();
    }

    protected override BaseState GetInitialState()
    {
        return wanderState;
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
        List<Transform> huntersTransformList = new List<Transform>();
        for (int i = 0; i < huntersTransform.childCount; i++) {
            huntersTransformList.Add(huntersTransform.GetChild(i));
        }
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
}
