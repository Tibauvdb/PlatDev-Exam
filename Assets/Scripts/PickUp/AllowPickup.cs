using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AllowPickup : MonoBehaviour {
    public bool PlayerInRange { get; set; }
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
            CheckPlayerState(other.gameObject);
            PlayerInRange = true;
        }
        else
            PlayerInRange = false;
            

        if(other.gameObject.tag == "AI" && Thrown == true)
        {
            other.GetComponent<BaseAIBehaviour>().IsAIKnockedOut = true;
            Thrown = false;
        }
    }

    private void CheckPlayerState(GameObject player)
    {
        PlayerBehaviour.States state = player.GetComponent<PlayerBehaviour>().State;

        //Allow player to pick up object
        if (Input.GetButtonDown(_input.A) && state != PlayerBehaviour.States.WalkingWithPickup)
        {
            player.GetComponent<PlayerBehaviour>().PickUpObject(this.gameObject);
            this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            Thrown = false;
        }

        //Preparing throw, as soon as trigger gets released, object will be thrown
        if (state == PlayerBehaviour.States.WalkingWithPickup && Input.GetAxis(_input.Triggers) != 0)
        {
            StartThrow();
        }

        //Throw object
        if (_isThrowing == true && Input.GetAxis(_input.Triggers) == 0)
        {
            Throw();
        }
    }

    private void StartThrow()
    {
        _objectThrowScript.enabled = true;
        this.gameObject.GetComponent<AllowPickup>().enabled = false;
        _isThrowing = true;

        _anim.SetTrigger("PreparingThrow");

        _objectThrowScript.ThrowAngle += 0.05f;
        _objectThrowScript.ThrowAngle = Mathf.Clamp(_objectThrowScript.ThrowAngle, 0, 45); //0,12
    }

    private void Throw()
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
