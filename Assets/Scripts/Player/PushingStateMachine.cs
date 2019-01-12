﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushingStateMachine : BaseStateMachineBehaviour {
    private PlayerBehaviour _bps;

    private void Awake()
    {
        _bps = SetPlayerBehaviour(_bps);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _bps.State = PlayerBehaviour.States.Walking;
	}
}
