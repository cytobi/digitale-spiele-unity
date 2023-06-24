using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunt : BaseState
{
    private NavSM _sm;

    public Hunt (NavSM stateMachine) : base("Hunt", stateMachine) {
        _sm = (NavSM) stateMachine;
    }

    public override void Enter() {
        base.Enter();
        // make agent run towards target
        _sm.agent.speed = 7f;
    }

    public override void UpdateLogic() {
        base.UpdateLogic();
        // update target for hunting
        _sm.agent.SetDestination(_sm.target.position);
        // if the target is far enough away, change to wander state
        if (Vector3.Distance(_sm.myTransform.position, _sm.target.position) > 5f) {
            _sm.ChangeState(_sm.wanderState);
        }
    }

    public override void Exit() {
        base.Exit();
        // make agent walk again
        _sm.agent.speed = 3.5f;
    }
}
