using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SittingStateMachine : BaseStateMachineBehaviour {
    [SerializeField]
    private PlayerBehaviour _bps;

    private PlayerBehaviour _backUpBPS;

    private void Awake()
    {
        _bps = SetPlayerBehaviour(_bps);
        _backUpBPS = GameObject.Find("Player").GetComponent<PlayerBehaviour>();
    }
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _backUpBPS.State = PlayerBehaviour.States.Sitting;

    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _backUpBPS.State = PlayerBehaviour.States.Walking;
    }
}
