using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine : StateMachineBehaviour {

    public BaseAIBehaviour AIBehaviour { get; set; }

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Vector3 aiLookPos = AIBehaviour.PlayerPosition + (Vector3.up * 1.5f);

        if (AIBehaviour.IsAIFollowing)
            SetLookAtPos(animator,aiLookPos);

        if (AIBehaviour.IsAILooking)
        {
            SetLookAtPos(animator,aiLookPos);
            //Make AI point at character
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKPosition(AvatarIKGoal.LeftHand,aiLookPos);          
        }
        else if (AIBehaviour.IsAILooking == false && AIBehaviour.name.Contains("Type02"))
        {
            //Reset left hand weight if AI loses track of player
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
        }
	}

    private void SetLookAtPos(Animator animator, Vector3 lookAt)
    {
        //When following player, make head follow player
        animator.SetLookAtWeight(1);
        animator.SetLookAtPosition(lookAt);
    }
}
