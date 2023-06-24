using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : BaseState
{
    private NavSM _sm;
    private int _wanderTimer = 0;
    private int _wanderTimerMax = 500;

    public Wander (NavSM stateMachine) : base("Wander", stateMachine) {
        _sm = (NavSM) stateMachine;
    }

    public override void Enter() {
        base.Enter();
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
        // if the target is close enough to the agent, change to seek state
        if (Vector3.Distance(_sm.myTransform.position, _sm.target.position) < 10f) {
            _sm.ChangeState(_sm.seekState);
        }
    }
}
