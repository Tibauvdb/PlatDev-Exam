using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowPush : MonoBehaviour {
    private Animator _anim;
    private InputManager _input;


    public float PushForce; //Base 10.5 F

    private bool _isPushing = false;

    private Transform _player;
    // Use this for initialization
    void Start () {
        _anim = GameObject.Find("Model").GetComponent<Animator>();
        _input = GameObject.Find("GameManager").GetComponent<InputManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (_isPushing)
            Push();
	}

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            PlayerBehaviour.States state = other.gameObject.GetComponent<PlayerBehaviour>().State;
            if (Input.GetButtonDown(_input.A) && state == PlayerBehaviour.States.Walking)
            {
                _anim.ResetTrigger("StopPushing");
                _anim.SetTrigger("IsPushing");

                other.gameObject.GetComponent<PlayerBehaviour>().State = PlayerBehaviour.States.PushingBox;
                _player = other.gameObject.transform;

                _isPushing = true;
                this.gameObject.transform.parent = _player;
            }

            if (Input.GetButtonDown(_input.A) && state == PlayerBehaviour.States.PushingBox)
            {
                _anim.ResetTrigger("IsPushing");
                _anim.SetTrigger("StopPushing");

                _isPushing = false;

                this.gameObject.transform.parent = null;
            }
        }
    }


    private void Push()
    {
        //if (_anim.GetFloat("VerticalVelocity") > 0.4f)
        //{
        //this.gameObject.GetComponent<Rigidbody>().AddForce(_player.forward * PushForce);
        //}
    }
}
