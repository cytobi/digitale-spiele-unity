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

        // change color to green
        _sm.myRenderer.material.color = Color.green;
    }

    public override void UpdateLogic() {
        base.UpdateLogic();

        // update target regularly
        _sm.agent.SetDestination(_sm.flockCenter());

        // apply force to keep agents apart
        float wantedDistance = _sm.wantedDistance;
        foreach (Transform t in _sm.flockTransformList) {
            if (t != _sm.myTransform) {
                Vector3 direction = _sm.myTransform.position - t.position;
                float distance = direction.magnitude;
                if (distance < wantedDistance) {
                    // force is proportional to distance
                    _sm.myRigidbody.AddForce(direction.normalized * (wantedDistance - distance) * 5f);
                }
            }
        }

        // change state to run away if a hunter is close enough
        if (_sm.closestHunterDistance() < 15f) {
            _sm.ChangeState(_sm.runAwayState);
        }
    }
}
