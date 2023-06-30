using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAway : BaseState
{
    private FlockSM _sm;

    public RunAway (FlockSM stateMachine) : base("RunAway", stateMachine) {
        _sm = (FlockSM) stateMachine;
    }

    public override void Enter() {
        base.Enter();

        // set target on enter
        _sm.agent.SetDestination(_sm.flockCenter() + _sm.flockCenter() - _sm.closestHunter().position);
    }

    public override void UpdateLogic() {
        base.UpdateLogic();

        // update target regularly
        _sm.agent.SetDestination(_sm.flockCenter() + _sm.flockCenter() - _sm.closestHunter().position);

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

        // change state to assemble if no hunter is close enough
        if (_sm.closestHunterDistance() > 10f) {
            _sm.ChangeState(_sm.assembleState);
        }
    }
}
