using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine : StateMachineBehaviour {

    public BaseAIBehaviour AIBehaviour;
	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	//override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (AIBehaviour.IsAILooking || AIBehaviour.IsAIFollowing)
        {
            Vector3 aiLookPos = AIBehaviour.PlayerPosition + (Vector3.up * 1.5f);
            //When following player, make head follow player
            animator.SetLookAtWeight(1);
            animator.SetLookAtPosition(aiLookPos);
            if (AIBehaviour.IsAILooking)
            {
                //Set AI to point at character
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, aiLookPos);
            }
        }
        else if (AIBehaviour.IsAILooking == false && AIBehaviour.name.Contains("Type02"))
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            Debug.Log("resetting weight");
            Debug.Log(animator.GetIKPositionWeight(AvatarIKGoal.LeftHand));
        }
	}
}
