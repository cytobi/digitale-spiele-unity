using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunt : BaseState
{
    private NavSM _sm;

    public Hunt (NavSM stateMachine) : base("Hunt", stateMachine) {
        _sm = (NavSM) stateMachine;
    }

    public override void UpdateLogic() {
        _sm.agent.SetDestination(_sm.target.position);
    }
}
