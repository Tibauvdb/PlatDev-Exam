using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithMouse : MonoBehaviour {
    public bool AllowRotation;
    public bool RaiseCam;

    private InputManager _input;
    public Transform _camPivot;

    private PlayerBehaviour _playerBH;
    private Transform _originalCameraPosition;
    private float _speed = 2.0f;

    private PlayerBehaviour.States _currState;
    // Use this for initialization
    void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        _input = GameObject.Find("GameManager").GetComponent<InputManager>();
        _playerBH = this.gameObject.GetComponent<PlayerBehaviour>();
        _originalCameraPosition = this.gameObject.transform.Find("KnockedOutCheck").transform;

    }
	
	// Update is called once per frame
	void Update () {
        _currState = _playerBH.State;

        if (AllowRotation)
            RotateCamera();

            RaiseCamera();
            BenchCamera();
    }
    private void RotateCamera()
    {
        Vector3 tempRot;
        if (_currState != PlayerBehaviour.States.Sitting)
        {
            tempRot = transform.localEulerAngles;
            tempRot.y += Input.GetAxis(_input.CamHorizontal) * _speed;
            transform.localEulerAngles = tempRot;
        }
        else
        {
            tempRot = _camPivot.localEulerAngles;
            tempRot.y += Input.GetAxis(_input.CamHorizontal) * _speed;
            _camPivot.localEulerAngles = tempRot;
           
        }

        Vector3 rotationCamPivot = _camPivot.transform.localEulerAngles;
        rotationCamPivot.x += Input.GetAxis(_input.CamVertical) * -_speed;
        rotationCamPivot.x = PlayerBehaviour.ClampAngle(rotationCamPivot.x, -10, 50);
        _camPivot.localEulerAngles = rotationCamPivot;

        if(_currState==PlayerBehaviour.States.Sitting)
            _camPivot.localEulerAngles = new Vector3(rotationCamPivot.x, tempRot.y-180, 0);
    }
    private void RaiseCamera()
    {
        //Gets called when player is moving with pickup | Pushing Box

        if(RaiseCam && _currState == PlayerBehaviour.States.PushingBox || _currState == PlayerBehaviour.States.WalkingWithPickup)
        {
            _camPivot.position = Vector3.Lerp(_camPivot.position, this.gameObject.transform.position + (Vector3.up * 2), Time.deltaTime);
        }
        else if(!RaiseCam)
        {
            _camPivot.position = Vector3.Lerp(_camPivot.position, _originalCameraPosition.position, Time.deltaTime);
        }
    }
    private void BenchCamera()
    {
        //Camera state for when player is sitting on a bench
        if(_currState == PlayerBehaviour.States.Sitting)
        {

        }
    }
}
