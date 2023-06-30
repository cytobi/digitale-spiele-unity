using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAway : BaseState
{
    private FlockSM _sm;
    private int _timer = 0;
    private int _timerMax = 100;

    public RunAway (FlockSM stateMachine) : base("RunAway", stateMachine) {
        _sm = (FlockSM) stateMachine;
    }

    public override void Enter() {
        base.Enter();

        // set target on enter
        _sm.agent.SetDestination(_sm.flockCenter() + runVector());

        // change color to blue
        _sm.myRenderer.material.color = Color.blue;
    }

    public override void UpdateLogic() {
        base.UpdateLogic();

        // update target regularly
        if (_timer > _timerMax) {
            _sm.agent.SetDestination(_sm.flockCenter() + runVector());
        } else {
            _timer++;
        }

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
        if (_sm.closestHunterDistance() > 18f) {
            _sm.ChangeState(_sm.assembleState);
        }
    }

    private Vector3 runVector() {
        Vector3 runVector = _sm.flockCenter() - weightedHunterPosition();
        runVector = runVector.normalized * 10f;
        return runVector;
    }

    private Vector3 weightedHunterPosition() {
        Vector3 weightedHunterPosition = Vector3.zero;
        float totalWeight = 0f;
        foreach (Transform t in _sm.huntersTransformList) {
            float distance = Vector3.Distance(_sm.flockCenter(), t.position);
            float weight = 1f / distance;
            weightedHunterPosition += t.position * weight;
            totalWeight += weight;
        }
        weightedHunterPosition /= totalWeight;
        return weightedHunterPosition;
    }
}
