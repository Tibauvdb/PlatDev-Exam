using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectThrow))]
[RequireComponent(typeof(LineRenderer))]
public class ThrowableBehaviour : MonoBehaviour {
    public bool Thrown { get; set; }

    private ObjectThrow _objectThrowScript;
    private InputManager _input;
    private bool _isThrowing = false;
    private Animator _anim;

    private void Start()
    {
        _anim = GameObject.Find("Model").GetComponent<Animator>();
        _input = GameObject.Find("GameManager").GetComponent<InputManager>();
        _objectThrowScript = this.gameObject.GetComponent<ObjectThrow>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            CheckPlayerStates(other.gameObject);
        }
        if (other.gameObject.tag == "AI" && Thrown == true)
        {
            other.GetComponent<BaseAIBehaviour>().IsAIKnockedOut = true;
            Thrown = false;
        }
    }

    public void StartThrow()
    {
        _objectThrowScript.enabled = true;
        this.gameObject.GetComponent<AllowPickup>().enabled = false;
        _isThrowing = true;

        _anim.SetTrigger("PreparingThrow");

        _objectThrowScript.ThrowAngle += 0.05f;
        _objectThrowScript.ThrowAngle = Mathf.Clamp(_objectThrowScript.ThrowAngle, 0, 45);
    }

    public void Throw()
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

    private void CheckPlayerStates(GameObject player)
    {
        PlayerBehaviour.States state = player.GetComponent<PlayerBehaviour>().State;

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
}
