using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowSitting : MonoBehaviour {
    private InputManager _input;
    private Animator _anim;
    private GameObject _player;

    private bool _allowInteraction = false;

    private Vector3 _sitPos;
    private enum SittingStates
    {
        Standing,
        StandToSit,
        Sitting,
        SitToStand
    }

    private SittingStates _currSitState = SittingStates.Standing;
    
    private void Start()
    {
        _input = GameObject.Find("GameManager").GetComponent<InputManager>();
        _anim = GameObject.Find("Model").GetComponent<Animator>();
    }

    private void Update()
    {
        Debug.Log(_currSitState);
        if (_allowInteraction == true)
        {
             switch (_currSitState)
            {
                case SittingStates.Standing:
                    _currSitState = SittingStates.StandToSit;
                    break;
                case SittingStates.StandToSit:
                    StartSit();
                    break;
                case SittingStates.Sitting:
                    break;
                case SittingStates.SitToStand:
                    StartStandingUp();
                    break;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if (Input.GetButtonDown(_input.A))
            {
                if (_allowInteraction && _currSitState == SittingStates.Sitting)
                    _currSitState = SittingStates.SitToStand;
                _player = other.gameObject;
                _allowInteraction = true;

            }
        }
    }

    private void StartSit()
    {
        //Start Standing to Sitting Animation
        _anim.SetTrigger("IsSitting");
        _anim.ResetTrigger("IsStandingUp");
        
        _player.transform.forward = Vector3.Lerp(_player.transform.forward, this.gameObject.transform.forward, Time.deltaTime*2);

        //Check if Animation is done playing
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Sitting")) 
        {
            _currSitState = SittingStates.Sitting;
            _player.GetComponent<PlayerBehaviour>().State = PlayerBehaviour.States.Sitting;
        }
    }

    private void StartStandingUp()
    {
        //Start Sitting to Standing Animation
        _anim.SetTrigger("IsStandingUp");
        _anim.ResetTrigger("IsSitting");

        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("ForwardLocomotion"))
        {
            _currSitState = SittingStates.Standing;
            _player.GetComponent<PlayerBehaviour>().State = PlayerBehaviour.States.Walking;
            _allowInteraction = false;
        }
    }
}
