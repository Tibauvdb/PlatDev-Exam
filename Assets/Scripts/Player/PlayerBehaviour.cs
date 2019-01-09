using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

//[RequireComponent(typeof(CharacterController))]
public class PlayerBehaviour : MonoBehaviour {

    //Create GetterSetter for Public Variables
    private InputManager _input;
    public IKGrabBehaviour IKGrab { get; set; }

    private PickUpStateMachine _pickupStateMachine;
    private ThrowingEndStateMachine _throwingStateMachine;
    private PushingStateMachine _pushingStateMachine;
    private StairWalkingStateMachine _stairWalkingStateMachine;
    private RotateWithMouse _cameraRotation;

    public enum States
    {
        Walking,
        Stairs,
        PushingBox,
        PickingUp,
        WalkingWithPickup,
        KnockedOut
    }

    //Create state
    [SerializeField] public States State { get; set; }
    

    [SerializeField] private float _acceleration; //[m/s2]
    [SerializeField] private float _drag;
    [SerializeField] private float _maximumXZVelocity = (30 * 1000) / (60 * 60); //[m/s] 30km/h
    [SerializeField] private float _jumpHeight; //[m]

    private Transform _absoluteTransform;
    private CharacterController _char;
    private Animator _anim;

    public Vector3 Velocity = Vector3.zero; // [m/s]

    [HideInInspector] public Vector3 InputMovementBase { get; set; }
    [HideInInspector] public Vector3 InputMovementBoxPushing { get; set; }

    private bool _pushBox = false;

    public RaycastHit Hit { get; set; }

    private GameObject _currentPickup;
    public bool PickingUp { get; set; }
    public bool IsKnockedOut { get; set; }
 

    void Start ()
        {
        //Set Components;
        #region Components
        _char = this.gameObject.GetComponent<CharacterController>();
        _absoluteTransform = Camera.main.transform;
        _anim = transform.GetChild(0).GetComponent<Animator>();
        _input = GameObject.Find("GameManager").GetComponent<InputManager>();
        IKGrab = this.gameObject.transform.GetChild(0).GetComponent<IKGrabBehaviour>();
        _cameraRotation = this.gameObject.GetComponent<RotateWithMouse>();
        #endregion
        //Set stateMachineBehaviour
        #region StateMachine Behaviours
        _pickupStateMachine = _anim.GetBehaviour<PickUpStateMachine>();
        _pickupStateMachine._bps = this;
        _throwingStateMachine = _anim.GetBehaviour<ThrowingEndStateMachine>();
        _throwingStateMachine._bps = this;
        _pushingStateMachine = _anim.GetBehaviour<PushingStateMachine>();
        _pushingStateMachine._bps = this;
        _stairWalkingStateMachine = _anim.GetBehaviour<StairWalkingStateMachine>();
        _stairWalkingStateMachine._bps = this;
        #endregion

        //Set Base State
        State = States.Walking;

        #region Dependencies
        //dependency error if charactercontroller is not attached
#if DEBUG
        Assert.IsNotNull(_char, "DEPENDENCY ERROR: CharacterController is missing from PlayerBehaviour");
#endif
        #endregion

    }

    private void Update()
        {
        //Get input from Any Horizontal or Vertical Axis
        //Normalize
        InputMovementBase = new Vector3(Input.GetAxis(_input.HorizontalAxis), 0,Input.GetAxis(_input.VerticalAxis)).normalized;  //Used for base movement

        #region Switch Case States
        switch (State){
            case States.Walking:

                //Coming out of PickUp v
                if (_anim.GetLayerWeight(1) == 1)
                {
                    _anim.SetLayerWeight(1, 0);
                    _currentPickup = null;
                }

                if (!_cameraRotation.AllowRotation)
                    _cameraRotation.AllowRotation = true;
                if(_cameraRotation.RaiseCam)
                    _cameraRotation.RaiseCam = false;
                break;

            case States.PickingUp:

                _cameraRotation.AllowRotation = false;

                if (_currentPickup != null && PickingUp == true)
                {
                    IKGrab.PickUp = _currentPickup.transform;
                    IKGrab.LookAtObj = _currentPickup.transform;
                }
                else if (_currentPickup != null && PickingUp == false)
                {
                    IKGrab.PickUp = null;
                }

                break;

            case States.PushingBox:

                RaiseCameraAndStopRotation();

                InputMovementBase = new Vector3(0, 0, Input.GetAxis(_input.VerticalAxis)).normalized;

                break;

            case States.WalkingWithPickup:
                _cameraRotation.AllowRotation = true;
                _cameraRotation.RaiseCam = true;
                break;

            case States.Stairs:

                break;

            case States.KnockedOut:

                break;

        }
        #endregion
    }

    void FixedUpdate ()
    {
        //Debug.Log(State);
        ApplyGround();
        ApplyGravity();

        if(State == States.Walking || State == States.WalkingWithPickup || State == States.PushingBox)
        {
            ApplyMovementBase();
        }


        ApplyDragOnGround();

        LimitXZVelocity();


        Vector3 XZvel = Vector3.Scale(Velocity, new Vector3(1, 0, 1));
        Vector3 localVelXZ = gameObject.transform.InverseTransformDirection(XZvel);
        
        #region SetAnimatorProperties
        _anim.SetFloat("VerticalVelocity", (localVelXZ.z * (_drag)) / _maximumXZVelocity);
        _anim.SetFloat("HorizontalVelocity", (localVelXZ.x * (_drag)) / _maximumXZVelocity);

        _anim.SetBool("PickUpObject", PickingUp);
        #endregion

    
        DoMovement();


    }

    //Get relative direction from camera
    private Vector3 RelativeDirection(Vector3 direction)
        {
        Quaternion relativeRot = Quaternion.LookRotation(direction);

        return relativeRot.eulerAngles;
        }

    //Apply gravity when player is on the ground
    private void ApplyGround()
        {
        if (_char.isGrounded)
            {
            //Gravity on ground
            Velocity -= Vector3.Project(Velocity, Physics.gravity);
            }
        }

    //Apply gravity when player is NOT on the ground
    private void ApplyGravity()
        {
        if (!_char.isGrounded)
            {
            //apply gravity when in the air
            Velocity += Physics.gravity * Time.deltaTime;
            }
        }

    //Apply movement when player is on the ground
    private void ApplyMovementBase()
        {
        if (_char.isGrounded)
            {
            //get relative rotation from camera
            Vector3 xzForward = Vector3.Scale(_absoluteTransform.forward, new Vector3(1, 0, 1));
            Quaternion relativeRot = Quaternion.LookRotation(xzForward);

            //move in relative direction
            Vector3 relativeMov = relativeRot * InputMovementBase;
            Velocity += relativeMov * _acceleration * Time.deltaTime; 
            }

        }

    //Limit player speed when moving
    private void LimitXZVelocity()
        {
        Vector3 yVel = Vector3.Scale(Velocity, Vector3.up);
        Vector3 xzVel = Vector3.Scale(Velocity, new Vector3(1, 0, 1));

        xzVel = Vector3.ClampMagnitude(xzVel, _maximumXZVelocity);

        Velocity = xzVel + yVel;
        }

    //Apply drag when player is on the ground
    private void ApplyDragOnGround()
        {
        if (_char.isGrounded)
            {
            Velocity = Velocity * (1 - _drag * Time.deltaTime); //Same as lerping
            }
        }

    //Final movement Function
    private void DoMovement()
        {
        //do velocity / movement on character controller
        Vector3 movement = Velocity * Time.deltaTime;
        _char.Move(movement);
        }

    //Pick up Object - Gets called from pickup-able objects - Not used atm  
    public void PickUpObject(GameObject pickup)
    {
        //Enter this script when player wants to pick up object in front of them
        _anim.Play("PickUp", 0);
        //Save current pickup
        _currentPickup = pickup;
        //Allow player to start throwing pickup;
        PickingUp = true;
        State = States.PickingUp;
        
    }

    public void KnockedOut()
    {
        //If AI gets too close, player will get knocked out
        IsKnockedOut = true;
        State = States.KnockedOut;
        _anim.SetTrigger("KnockedOut");
    }

    private void RaiseCameraAndStopRotation()
    {
        if (_cameraRotation.AllowRotation == true)
            _cameraRotation.AllowRotation = false;

        _cameraRotation.RaiseCam = true;
    }
    //Found On Internet - Not Mine
    //Function to clamp angles | Used to clamp rotation of camera
    public static float ClampAngle(float angle, float min, float max)
        {
        angle = Mathf.Repeat(angle, 360);
        min = Mathf.Repeat(min, 360);
        max = Mathf.Repeat(max, 360);
        bool inverse = false;
        var tmin = min;
        var tangle = angle;
        if (min > 180)
            {
            inverse = !inverse;
            tmin -= 180;
            }
        if (angle > 180)
            {
            inverse = !inverse;
            tangle -= 180;
            }
        var result = !inverse ? tangle > tmin : tangle < tmin;
        if (!result)
            angle = min;

        inverse = false;
        tangle = angle;
        var tmax = max;
        if (angle > 180)
            {
            inverse = !inverse;
            tangle -= 180;
            }
        if (max > 180)
            {
            inverse = !inverse;
            tmax -= 180;
            }

        result = !inverse ? tangle < tmax : tangle > tmax;
        if (!result)
            angle = max;
        return angle;
        }
    }
