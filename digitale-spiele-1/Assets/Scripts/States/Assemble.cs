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
        _sm.agent.SetDestination(_sm.flockCenter());
    }

    public override void UpdateLogic() {
        base.UpdateLogic();

        // update target regularly
        _sm.agent.SetDestination(_sm.flockCenter());

        // apply force to keep agents apart
        float wantedDistance = 3f;
        foreach (Transform t in _sm.flockTransformList) {
            if (t != _sm.myTransform) {
                Vector3 direction = _sm.myTransform.position - t.position;
                float distance = direction.magnitude;
                if (distance < wantedDistance) {
                    Debug.Log(distance);
                    // force is proportional to distance
                    _sm.myRigidbody.AddForce(direction.normalized * (wantedDistance - distance) * 5f);
                }
            }
        }
        
    }
}
