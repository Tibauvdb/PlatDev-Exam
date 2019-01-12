using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBehaviour : MonoBehaviour {
    private Animator _anim;
    private InputManager _input;

    private float _pushForce = 10.5f; //Base 10.5 F

    private bool _isPushing = false;

    private Transform _player;
    // Use this for initialization
    void Start () {
        _anim = GameObject.Find("Model").GetComponent<Animator>();
        _input = GameObject.Find("GameManager").GetComponent<InputManager>();
	}
	
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            PlayerBehaviour.States state = other.gameObject.GetComponent<PlayerBehaviour>().State;
            if (Input.GetButtonDown(_input.A) && state == PlayerBehaviour.States.Walking)
            {
                PushingBox(other);
            }

            if (Input.GetButtonDown(_input.A) && state == PlayerBehaviour.States.PushingBox)
            {
                StopPushingBox();
            }
        }

        if(other.gameObject.tag == "AI" && _isPushing)
        {
            Vector3 playerVel = _player.gameObject.GetComponent<PlayerBehaviour>().Velocity;

            if (playerVel.x > 0 || playerVel.z > 0)
            {
                other.gameObject.GetComponent<BaseAIBehaviour>().IsAIKnockedOut = true;
            }
        }
    }

    private void PushingBox(Collider other)
    {
        _anim.ResetTrigger("StopPushing");
        _anim.SetTrigger("IsPushing");

        other.gameObject.GetComponent<PlayerBehaviour>().State = PlayerBehaviour.States.PushingBox;
        _player = other.gameObject.transform;

        _isPushing = true;
        this.gameObject.transform.parent = _player;
    }

    private void StopPushingBox()
    {
        _anim.ResetTrigger("IsPushing");
        _anim.SetTrigger("StopPushing");

        _isPushing = false;

        this.gameObject.transform.parent = null;
    }
}
