﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpStateMachine : BaseStateMachineBehaviour
{
    public Transform PickUp { get; set; }
    public Transform LookAtObj { get; set; }

    private PlayerBehaviour _bps;

    private float _lerpCount = 0.0f;
    private float _animatorCutTime = 0.34f;
    private void Awake()
    {
        _bps =  SetPlayerBehaviour(_bps);
    }
    
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



    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (LookAtObj != null)
        {
            //Set Look At
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= _animatorCutTime)
            {
                animator.SetLookAtWeight(1);
                animator.SetLookAtPosition(LookAtObj.position);  
            }
        }

        if (PickUp != null)
        {
            //Set Pick Up
            if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= _animatorCutTime)
            {
                SetPickUpIK(animator);
            }
            else
            {
                PickUp.parent = animator.GetBoneTransform(HumanBodyBones.RightHand);
                PickUp.position = animator.GetBoneTransform(HumanBodyBones.RightHand).position;
            } 

        }
    }
    private void SetPickUpIK(Animator animator )
    {
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, Mathf.Lerp(0, 1, _lerpCount));
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, Mathf.Lerp(0, 1, _lerpCount));
        animator.SetIKPosition(AvatarIKGoal.RightHand, PickUp.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, PickUp.rotation);

        if (_lerpCount < 1.0f)
            _lerpCount += Time.deltaTime/2;
    }

    public void Reset()
    {
        PickUp = null;
        LookAtObj = null;
    }

   /* public override void OnStateIK(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        GameObject currentPick;

        currentPick = _bps.CurrentPickUp;

        SetPickUpIK(animator);


        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKPosition(AvatarIKGoal.RightHand, currentPick.transform.position);
    }*/
}
