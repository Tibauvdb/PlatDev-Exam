using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

//[RequireComponent(typeof(CharacterController))]
public class BasePlayerScript : MonoBehaviour { 
    public InputManager _input;

    public enum State
    {
        Walking,
        Stairs,
        PushingBox
    }
    //Create state and set Base state to walking
    public State state = State.Walking;
    
    [SerializeField] private float _acceleration; //[m/s2]
    [SerializeField] private float _drag;
    [SerializeField] private float _maximumXZVelocity = (30 * 1000) / (60 * 60); //[m/s] 30km/h
    [SerializeField] private float _jumpHeight; //[m]
    [SerializeField] private Transform _rayCastTransform;

    private Transform _absoluteTransform;
    private CharacterController _char;
    private Animator _anim;

    [HideInInspector] public Vector3 Velocity = Vector3.zero; // [m/s]

    [HideInInspector] public Vector3 InputMovementBase;
    [HideInInspector] public Vector3 InputMovementBoxPushing;

    private bool _pushBox = false;
    public RaycastHit Hit;

    private GameObject _currentPickup;
    private bool _pickingUp;
    private bool _canThrow;

    void Start ()
        {
        //attach components
        _char = GetComponent<CharacterController>();
        _absoluteTransform = Camera.main.transform;
        _anim = transform.GetChild(0).GetComponent<Animator>();
        _input = GameObject.Find("GameManager").GetComponent<InputManager>();

        #region Dependencies
        //dependency error if charactercontroller is not attached
#if DEBUG
        Assert.IsNotNull(_char, "DEPENDENCY ERROR: CharacterController missing from PlayerScript");
#endif
        #endregion

    }

    private void Update()
        {
            //Get input from Any Horizontal or Vertical Axis
            //Normalize
            InputMovementBase = new Vector3(Input.GetAxis(_input.HorizontalAxis), 0, Input.GetAxis(_input.VerticalAxis)).normalized;  //Used for base movement

           /* if (Input.GetButtonDown(_input.A))
            {
               // _pushBox = true;
            }*/

            if (Input.GetAxis(_input.Triggers) > 0)
            {
                
            }
            


        }

    void FixedUpdate ()
        {
        
            ApplyGround();
            ApplyGravity();

            if(state == State.Walking)
            {
                ApplyMovementBase();
            }

            ApplyDragOnGround();

            //Push Box
            PushBox();

            LimitXZVelocity();


            Vector3 XZvel = Vector3.Scale(Velocity, new Vector3(1, 0, 1));
            Vector3 localVelXZ = gameObject.transform.InverseTransformDirection(XZvel);

            _anim.SetFloat("VerticalVelocity", (localVelXZ.z * (_drag)) / _maximumXZVelocity);
            _anim.SetFloat("HorizontalVelocity", (localVelXZ.x * (_drag)) / _maximumXZVelocity);

            _anim.SetBool("PickUpObject", _pickingUp);

            DoMovement();
        
        }

    //Get relative direction from camera
    private Vector3 RelativeDirection(Vector3 direction)
        {
        //Vector3 xzForward = Vector3.Scale(_absoluteTransform.forward, new Vector3(1, 0, 1));
        Quaternion relativeRot = Quaternion.LookRotation(direction);

        return relativeRot.eulerAngles;
        }

    //Apply gravity when player is on the ground
    private void ApplyGround()
        {
        if (_char.isGrounded)
            {
            //ground velocity
            Velocity -= Vector3.Project(Velocity, Physics.gravity);
            }
        }

    //Apply gravity when player is NOT on the ground
    private void ApplyGravity()
        {
        if (!_char.isGrounded)
            {
            //apply gravity
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

    //Allow Player To Push Boxes Around
    private void PushBox()
    {
        //if Player pressed A with box in front of him - Go to push State
        if(_pushBox == true)
        {
            RaycastHit hitInfo;
            //Check if box is in front of player | Possibly replace with spherecast to check all around the player
            if (Physics.Raycast(this.gameObject.transform.position, this.gameObject.transform.forward, out hitInfo,5, -1)){
                if(hitInfo.transform.gameObject.tag == "Box")
                {
                    Debug.Log("Entering PushBox");
                    state = State.PushingBox;
                    Rigidbody currentBoxRB = hitInfo.transform.gameObject.GetComponent<Rigidbody>();
                    currentBoxRB.AddForce(this.gameObject.transform.forward * 10f, ForceMode.Acceleration);
                    //Make sure player can only move in the forward of the box | Abs will make it so the movement isn't negative 
                    //InputMovementBoxPushing = new Vector3(Mathf.Abs((Input.GetAxisRaw("Horizontal") * hitInfo.transform.forward.x)), 0, Mathf.Abs(( Input.GetAxisRaw("Vertical") * hitInfo.transform.forward.z))).normalized;
                    state = State.Walking;

                }
            }
           // _pushBox = false;
        }
    }

    //Pick up Object - Gets called from pickup-able objects
    public void PickUpObject(GameObject pickup)
    {
        //Enter this script when player wants to pick up object in front of them

        //Play Pickup Animation
        //_anim.SetLayerWeight(1, 1);
        //Save current pickup
        _currentPickup = pickup;

        //Allow player to start throwing pickup;
        _pickingUp = true;
        Animation animation;
        animation.
        if()
        {
            _anim.SetLayerWeight(1, 1);
            _pickingUp = false;
            _canThrow = true;
        }

    }

    

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
