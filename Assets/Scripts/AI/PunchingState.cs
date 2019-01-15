using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchingState : BaseStateMachineBehaviour {
    private BaseAIBehaviour _ais;
    private PlayerBehaviour _pbs;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _ais = SetAIBehaviour(_ais, animator);
        _pbs = SetPlayerBehaviour(_pbs);
    }

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //if (_ais != null)
        //{
        //    SetHandIK(animator);
        //}
	}
    
    private void SetHandIK(Animator animator)
    {
        //Set right hand to player position
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKPosition(AvatarIKGoal.RightHand, _pbs.gameObject.transform.position+Vector3.up);
    }
}
