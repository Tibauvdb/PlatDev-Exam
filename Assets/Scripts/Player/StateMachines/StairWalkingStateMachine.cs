using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairWalkingStateMachine : BaseStateMachineBehaviour {
    public PlayerBehaviour _bps;
    
    private Transform _leftFoot;
    private Transform _rightFoot;
    private Vector3 _leftFootPos;
    private Vector3 _rightFootPos;
    private Quaternion _leftFootRot;
    private Quaternion _rightFootRot;

    private float _leftFootWeight;
    private float _rightFootWeight;

    private void Awake()
    {
        _bps = SetPlayerBehaviour(_bps);
    }
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _leftFoot = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
        _rightFoot = animator.GetBoneTransform(HumanBodyBones.RightFoot);
    }

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
        _leftFootWeight = animator.GetFloat("LeftFoot");
        _rightFootWeight = animator.GetFloat("RightFoot");


        FloorHitPosition(_leftFoot, ref _leftFootPos, ref _leftFootRot, Vector3.up);
        FloorHitPosition(_rightFoot, ref _rightFootPos, ref _rightFootRot, Vector3.up);


        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, _leftFootWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, _rightFootWeight);

        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, _leftFootWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, _rightFootWeight);

        // set the position of the feet
        animator.SetIKPosition(AvatarIKGoal.LeftFoot, _leftFootPos);
        animator.SetIKPosition(AvatarIKGoal.RightFoot, _rightFootPos);

        // set the rotation of the feet
        animator.SetIKRotation(AvatarIKGoal.LeftFoot, _leftFootRot);
        animator.SetIKRotation(AvatarIKGoal.RightFoot, _rightFootRot);
    }
    private void StairIkPerFoot(Animator animator,AvatarIKGoal foot)
    {
        Debug.Log("Entering OnStateIK | StairWalking");
        //If player is walking on stairs --> Place foot on right positions
        RaycastHit hit;
        if (Physics.Raycast(animator.GetIKPosition(foot), Vector3.down, out hit, 0.5f))
        {
            Debug.Log("Raycast Hit");
            //if it hits something
            animator.SetIKPosition(foot, hit.point);
            animator.SetIKPositionWeight(foot, 1);
        }
        Debug.DrawRay(animator.GetIKPosition(foot), Vector3.down, Color.red);
    }


    void FloorHitPosition(Transform t, ref Vector3 targetPos, ref Quaternion targetRot, Vector3 direction)
    {
        Debug.Log("Calling FloorHitPos");
        RaycastHit hit;
        Vector3 origin = t.position;

        //Add an offset
        origin += direction * 0.3f;

        //Raycast
        Debug.DrawRay(origin, -direction, Color.red);
        if(Physics.Raycast(origin,-direction,out hit, 3))
        {
            targetPos = hit.point;
            //rotate based around normal
            GameObject player = GameObject.Find("Player");
            Quaternion rot = Quaternion.LookRotation(player.transform.forward);
            targetRot = Quaternion.FromToRotation(Vector3.up, hit.normal) * rot;
        }
    }
}
