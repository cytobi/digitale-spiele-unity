using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : BaseState
{
    private NavSM _sm;
    private int _seekTimer = 0;
    private int _seekTimerMax = 300;

    public Seek (NavSM stateMachine) : base("Seek", stateMachine) {
        _sm = (NavSM) stateMachine;
    }

    public override void Enter() {
        base.Enter();

        // set target on enter
        _sm.agent.SetDestination(_sm.target.position + Random.insideUnitSphere * 2f);

        // change color to yellow
        _sm.myRenderer.material.color = Color.yellow;
        _sm.myLight.color = Color.yellow;
    }

    public override void UpdateLogic() {
        base.UpdateLogic();

        // update target for seeking after a certain amount of time
        if (_seekTimer > _seekTimerMax) {
            _seekTimer = 0;
            _sm.agent.SetDestination(_sm.target.position + Random.insideUnitSphere * 5f);
        } else {
            _seekTimer++;
        }

        // if the target is far enough away, change to wander state
        if (Vector3.Distance(_sm.myTransform.position, _sm.target.position) > 20f) {
            _sm.ChangeState(_sm.wanderState);
        }

        // if the target is close enough to the agent, change to hunt state
        if (Vector3.Distance(_sm.myTransform.position, _sm.target.position) < 5f) {
            _sm.ChangeState(_sm.huntState);
        }
    }
}
