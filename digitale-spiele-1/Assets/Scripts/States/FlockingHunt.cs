using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingHunt : BaseState
{
    private HunterSM _sm;

    public FlockingHunt (HunterSM stateMachine) : base("FlockingHunt", stateMachine) {
        _sm = (HunterSM) stateMachine;
    }

    public override void Enter() {
        base.Enter();

        // set target on enter
        _sm.agent.SetDestination(_sm.flockCenter());
    }

    public override void UpdateLogic() {
        base.UpdateLogic();

        // update target for hunting
        _sm.agent.SetDestination(_sm.flockCenter());

        // if this is no longer the closest hunter to the flock, change to wander state
        if (_sm.closestHunter() != _sm.myTransform) {
            _sm.ChangeState(_sm.wanderState);
        }
    }
}
