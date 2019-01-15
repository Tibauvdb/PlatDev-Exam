using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushingStateMachine : BaseStateMachineBehaviour {
    private PlayerBehaviour _bps;

    private Transform _leftShoulder;
    private Transform _rightShoulder;
    private void Awake()
    {
        _bps = SetPlayerBehaviour(_bps);    
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.SetLayerWeight(2, 1);

    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _bps.State = PlayerBehaviour.States.Walking;

        animator.SetLayerWeight(2, 0);
	}

    public override void OnStateIK(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        _leftShoulder = animator.GetBoneTransform(HumanBodyBones.LeftShoulder);
        _rightShoulder = animator.GetBoneTransform(HumanBodyBones.RightShoulder);

        CheckRaycast(_leftShoulder, AvatarIKGoal.LeftHand, animator);
        CheckRaycast(_rightShoulder, AvatarIKGoal.RightHand, animator);

    }

    private void CheckRaycast(Transform startPos,AvatarIKGoal ikGoal,Animator animator)
    {
        RaycastHit hit;
        if(Physics.Raycast(startPos.position,_bps.gameObject.transform.forward,out hit, 1))
        {
            Vector3 targetpos = hit.point;
            SetIK(animator, ikGoal, targetpos);
        }
    }

    private void SetIK(Animator animator,AvatarIKGoal ikGoal,Vector3 targetPos)
    {
        animator.SetIKPositionWeight(ikGoal, 1);
        animator.SetIKPosition(ikGoal, targetPos);
    }
}
