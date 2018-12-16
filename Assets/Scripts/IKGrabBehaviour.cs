using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKGrabBehaviour : MonoBehaviour {
    //[SerializeField] private bool _ikActive = false;

    public Animator Anim { get; set; }
    public Transform PickUp { get; set; }
    public Transform LookAtObj { get; set; }

    public GameObject RightHand;
    #region IKproperties
    private Transform _rightHand;
    private float _rightHandWeight;
    private Vector3 _rightHandPos;
    #endregion

    private float _lerpCount = 0.0f;

    private void Start()
    {
        Anim = this.gameObject.GetComponent<Animator>();
    }

    public void OnAnimatorIK(int layerIndex)
    {
        Debug.Log("Entering OnAnimatorIK");

        //If there is an object to look at, set the lookAt Position and weight
        if (LookAtObj != null)
        {
            if(Anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.34f)
            {

            Anim.SetLookAtWeight(1);
            Anim.SetLookAtPosition(LookAtObj.position);
            }
        }

        //Set right Hand position and weight if one has been given
        if (PickUp != null)
        {
            //when entering state, weight will always be 0
            if (Anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.34f)//0.34
            {
                Anim.SetIKPositionWeight(AvatarIKGoal.RightHand, Mathf.Lerp(_lerpCount, 1, Time.deltaTime));
                Anim.SetIKRotationWeight(AvatarIKGoal.RightHand, Mathf.Lerp(_lerpCount, 1, Time.deltaTime));
                Anim.SetIKPosition(AvatarIKGoal.RightHand, PickUp.position);
                Anim.SetIKRotation(AvatarIKGoal.RightHand, PickUp.rotation);
                if (_lerpCount < 1.0f)
                {
                    _lerpCount += 0.005f;
                }
            Debug.Log("Term 1 Weight: "+ Anim.GetIKPositionWeight(AvatarIKGoal.RightHand));
            Debug.Log("Term 1 Pos: " + Anim.GetIKPosition(AvatarIKGoal.RightHand));
            }

            if(Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.34f)
            {
                PickUp.parent = RightHand.transform;
                PickUp.position = RightHand.transform.position;
            }






            //as soon as i exit the above statement weight resets to 0
            //weight gets reset somewhere????????????
            /*else if(Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.34f){
             //   //Fully resets Animation for some reason WEIGHT DOESNT MATTER FOR SOME OTHER REASON
             //   Debug.Log("Resetting Arm" + _lerpCount);
               Anim.SetIKPositionWeight(AvatarIKGoal.RightHand, Mathf.Lerp(_lerpCount, 0, Time.deltaTime));
               Anim.SetIKRotationWeight(AvatarIKGoal.RightHand, Mathf.Lerp(_lerpCount, 0, Time.deltaTime));
               if (_lerpCount >0.0f)
               {
                   _lerpCount -= 0.01f;
               }
               Debug.Log("Term 2 Weight: " +Anim.GetIKPositionWeight(AvatarIKGoal.RightHand));
               Debug.Log("Term 2 Pos: " + Anim.GetIKPosition(AvatarIKGoal.RightHand));
                //   //Anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                //   //Anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            }*/
        }
    }

    public void Reset()
    {
        PickUp = null;
        LookAtObj = null;
    }
}
