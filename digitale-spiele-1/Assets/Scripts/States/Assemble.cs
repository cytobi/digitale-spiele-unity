using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assemble : BaseState
{
    private FlockSM _sm;

    public Assemble (FlockSM stateMachine) : base("Assemble", stateMachine) {
        _sm = (FlockSM) stateMachine;
    }

    public override void Enter() {
        base.Enter();

        // set target on enter
        Vector3 center = calculateCenter(_sm.flockTransform);
        _sm.agent.SetDestination(center);
    }

    public override void UpdateLogic() {
        base.UpdateLogic();

        // update target regularly
        Vector3 center = calculateCenter(_sm.flockTransform);
        _sm.agent.SetDestination(center);
    }

    private Vector3 calculateCenter(Transform flockTransform) {
        // calculate center of flock
        List<Transform> flockTransforms = new List<Transform>();
        for (int i = 0; i < _sm.flockTransform.childCount; i++) {
            flockTransforms.Add(_sm.flockTransform.GetChild(i));
        }
        Vector3 center = Vector3.zero;
        foreach (Transform t in flockTransforms) {
            center += t.position;
        }
        center /= _sm.flockTransform.childCount;
        return center;
    }
}
