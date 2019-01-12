using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpStateMachine : StateMachineBehaviour
{
    public PlayerBehaviour _bps { get; set; }
    public Transform PickUp { get; set; }
    public Transform LookAtObj { get; set; }

    private float _lerpCount = 0.0f;
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
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        _bps.PickingUp = false;
        animator.SetBool("PickUpObject", _bps.PickingUp);
        Reset();
        //Mask Layer so upper body is still holding object
        animator.SetLayerWeight(1, 1);

        //Allow Player to walk again
        _bps.State = PlayerBehaviour.States.WalkingWithPickup;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (LookAtObj != null)
        {
            //Set Look At
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.34f)
            {
                animator.SetLookAtWeight(1);
                animator.SetLookAtPosition(LookAtObj.position);  
            }
        }

        if (PickUp != null)
        {
            //Set Pick Up
            if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.34f)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, Mathf.Lerp(_lerpCount, 1, Time.deltaTime));
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, Mathf.Lerp(_lerpCount, 1, Time.deltaTime));
                animator.SetIKPosition(AvatarIKGoal.RightHand, PickUp.position);
                animator.SetIKRotation(AvatarIKGoal.RightHand, PickUp.rotation);

                if (_lerpCount < 1.0f)
                    _lerpCount += 0.005f;
            }
            else
            {
                PickUp.parent = animator.GetBoneTransform(HumanBodyBones.RightHand);
                PickUp.position = animator.GetBoneTransform(HumanBodyBones.RightHand).position;
            } 

        }
    }

    public void Reset()
    {
        PickUp = null;
        LookAtObj = null;
    }
}
