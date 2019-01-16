using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitToStandStateMachine : BaseStateMachineBehaviour {
    private PlayerBehaviour _bps;
    private void Awake()
    {
        _bps = SetPlayerBehaviour(_bps);
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _bps.AllowDoMovement = false;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _bps.AllowDoMovement = true;
    }
}
