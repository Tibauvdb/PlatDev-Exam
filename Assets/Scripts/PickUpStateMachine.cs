using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpStateMachine : StateMachineBehaviour {

    public PlayerBehaviour _bps;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
   //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   //{
   //
   //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        //Happens at start not at end
        Debug.Log("Exiting Animation");

        _bps.PickingUp = false;
        animator.SetBool("PickUpObject", _bps.PickingUp);

        //Mask Layer so upper body is still holding object
        animator.SetLayerWeight(1, 1);

        //Allow Player to walk again
        _bps.State = PlayerBehaviour.States.Walking;
    }

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Debug.Log("Entering OnStateIK");
	}
}
