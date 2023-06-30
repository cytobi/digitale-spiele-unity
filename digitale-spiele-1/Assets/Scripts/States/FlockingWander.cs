using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingWander : BaseState
{
    private HunterSM _sm;
    private int _wanderTimer = 0;
    private int _wanderTimerMax = Random.Range(300, 500);

    public FlockingWander (HunterSM stateMachine) : base("FlockingWander", stateMachine) {
        _sm = (HunterSM) stateMachine;
    }

    public override void Enter() {
        base.Enter();

        // set target on enter
        _sm.agent.SetDestination(_sm.myTransform.position + Random.insideUnitSphere * 30f);
    }

    public override void UpdateLogic() {
        base.UpdateLogic();

        // update target for wandering after a certain amount of time
        if (_wanderTimer > _wanderTimerMax) {
            _wanderTimer = 0;
            _sm.agent.SetDestination(_sm.myTransform.position + Random.insideUnitSphere * 10f);
        } else {
            _wanderTimer++;
        }

        // if this is the closest hunter to the flock, change to hunt state
        if (_sm.closestHunter() == _sm.myTransform) {
            _sm.ChangeState(_sm.huntState);
        }
    }
}
