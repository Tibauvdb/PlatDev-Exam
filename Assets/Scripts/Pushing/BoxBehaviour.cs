using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBehaviour : Avatar {
    private InputManager _input;
    private Animator _anim;
    private Rigidbody _rb;

    [SerializeField]
    private float _pushForce = 10.5f; //Base 10.5 F
    private float _maxXZVel = 2;
    private float _forwardVelocity;

    [SerializeField]
    private bool _isPushing = false;
    [SerializeField]
    private bool _stopPush = false;

    private bool _allowPush = true;

    private Transform _player;
    private Vector3 _targetTrans;
    private float _lerpCounter;

    private bool _lerpForward;

    private Animation _legAnimation;

    // Use this for initialization
    void Start () {
        _input = GameObject.Find("GameManager").GetComponent<InputManager>();
        _anim = GameObject.Find("Model").GetComponent<Animator>();
        _rb = this.GetComponent<Rigidbody>();
	}

    private void Update()
    {
        _rb.velocity = LimitXZVel(_rb.velocity, _maxXZVel);

        if (_lerpForward)
            LerpForward(_player,_targetTrans);
    }

    private void FixedUpdate()
    {
        if (_isPushing && !_stopPush)
            PushBox();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player" && _allowPush)
        {
            CheckPlayerState(other.gameObject);
        }

        if (other.gameObject.tag == "AI" && _isPushing)
        {
            Vector3 playerVel = _player.gameObject.GetComponent<PlayerBehaviour>().Velocity;

            if (playerVel.x > 0 || playerVel.z > 0)
            {
                other.gameObject.GetComponent<BaseAIBehaviour>().IsAIKnockedOut = true;
            }
        }

        if(other.gameObject.tag == "PickUp")
        {
            _allowPush = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<PlayerBehaviour>().State == PlayerBehaviour.States.PushingBox)
        {
            PushBoxToWalking(other.gameObject);
        }

        if(other.gameObject.tag == "PickUp")
        {
            _allowPush = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Level" && !_isPushing) //if THIS box isn't getting pushed and hits a wall --> don't allow movement
        {
            //_stopPush = true;
        }
        if(collision.gameObject.tag == "Box")
        {
            Rigidbody tempRB = collision.gameObject.GetComponent<Rigidbody>();

            if(_isPushing)
                tempRB.AddForce(_player.transform.forward * _pushForce, ForceMode.Force);
        }
    }

    private void CheckPlayerState(GameObject player)
    {
        PlayerBehaviour.States state = player.GetComponent<PlayerBehaviour>().State;

        if (Input.GetButtonDown(_input.A))
        {
            if (state == PlayerBehaviour.States.Walking)
            {
                WalkingToPushBox(player);
            }
            else if (state == PlayerBehaviour.States.PushingBox)
            {
                PushBoxToWalking(player);
            }
        }

        if (_stopPush)
            player.GetComponent<PlayerBehaviour>().AllowDoMovement = false;
    }

    private void PushBox()
    {
        _forwardVelocity = GetPlayerVelocity();

        //Use player's forward, the Force the box will be pushed with and player's Velocity according to forward to push the box
        this.GetComponent<Rigidbody>().AddForce(_player.transform.forward * _pushForce  * (_forwardVelocity/2), ForceMode.Force);

        //Set feet movement on second layer
        if (_forwardVelocity <= 0.1f)
            _anim.SetFloat("SpeedMirror", 0);
        else
            _anim.SetFloat("SpeedMirror", 1);


    }

    private void WalkingToPushBox(GameObject player)
    {
        _anim.ResetTrigger("StopPushing");
        _anim.SetTrigger("IsPushing");

        _player = player.transform;
        _isPushing = true;

        player.GetComponent<PlayerBehaviour>().State = PlayerBehaviour.States.PushingBox;
        ChangePlayerForward(_player);
    }

    private void PushBoxToWalking(GameObject player)
    {
        _anim.ResetTrigger("IsPushing");
        _anim.SetTrigger("StopPushing");

        _isPushing = false;

        player.GetComponent<PlayerBehaviour>().State = PlayerBehaviour.States.Walking;
    }

    private float GetPlayerVelocity()
    {
        Vector3 _playerVel = _player.GetComponent<PlayerBehaviour>().Velocity;

        float forwardVel;
        if (Mathf.Abs(_playerVel.x) > Mathf.Abs(_playerVel.z))
            forwardVel = _playerVel.x;
        else
            forwardVel = _playerVel.z;

        forwardVel = Mathf.Abs(forwardVel);
        return forwardVel;
    }

    private void ChangePlayerForward(Transform playerTrans)
    {
        RaycastHit hit;
        if(Physics.Linecast(playerTrans.position, this.gameObject.transform.position,out hit))
        {
            _targetTrans =  -hit.normal;
            _lerpForward = true;
        }
    }

    private void LerpForward(Transform current,Vector3 target)
    {
        _lerpCounter += Time.deltaTime;
       current.forward = Vector3.Lerp(current.forward, target, _lerpCounter);

        if (_lerpCounter>1)
        {
            _lerpForward = false;
            _lerpCounter = 0;
        }
    }
}
