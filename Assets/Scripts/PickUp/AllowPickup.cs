using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AllowPickup : MonoBehaviour {

    private Animator _anim;
    private InputManager _input;
    private bool _isThrowing = false;
    public bool Thrown = false;

    private ObjectThrow _objectThrowScript;
    private void Start()
    {
        _anim = GameObject.Find("Model").GetComponent<Animator>();
        _input = GameObject.Find("GameManager").GetComponent<InputManager>();
        _objectThrowScript = this.gameObject.GetComponent<ObjectThrow>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerBehaviour.States state = other.gameObject.GetComponent<PlayerBehaviour>().State;
            if (Input.GetButtonDown(_input.A) && other.gameObject.GetComponent<PlayerBehaviour>().State != PlayerBehaviour.States.WalkingWithPickup)
            {
                other.gameObject.GetComponent<PlayerBehaviour>().PickUpObject(this.gameObject);
                this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                Thrown = false;
            }
            //If player presses trigger and is walkingwithPickup -> activate throw script and deactivate this one
            if(state == PlayerBehaviour.States.WalkingWithPickup && Input.GetAxis(_input.Triggers)!=0)
            {
                //Start Throwing, as soon as trigger gets released, object will be thrown

                _objectThrowScript.enabled = true;
                this.gameObject.GetComponent<AllowPickup>().enabled = false;
                _isThrowing = true;

                _anim.SetTrigger("PreparingThrow");

                _objectThrowScript.ThrowAngle += 0.05f;
                _objectThrowScript.ThrowAngle =  Mathf.Clamp(_objectThrowScript.ThrowAngle, 0, 45); //0,12
            }

            //If Player Releases trigger, throw the object
            if(_isThrowing==true && Input.GetAxis(_input.Triggers) == 0)
            {
                _anim.ResetTrigger("PreparingThrow");
                _anim.SetTrigger("IsThrowing");

                //Throw Object
                this.gameObject.GetComponent<ObjectThrow>().Throwing = true;
                this.gameObject.transform.parent = null;
                this.gameObject.GetComponent<Rigidbody>().isKinematic = false;

                //Reset
                _isThrowing = false;
                Thrown = true;
            }
        }

        else if(other.gameObject.tag == "AI" && Thrown == true)
        {
            other.GetComponent<BaseAIBehaviour>().IsAIKnockedOut = true;
            Thrown = false;
        }
    }
}
