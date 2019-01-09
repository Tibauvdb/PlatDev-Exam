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
	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        _input = GameObject.Find("GameManager").GetComponent<InputManager>();
        _playerBH = this.gameObject.GetComponent<PlayerBehaviour>();
        _originalCameraPosition = this.gameObject.transform.Find("KnockedOutCheck").transform;
	}
	
	// Update is called once per frame
	void Update () {
        if (AllowRotation)
            RotateCamera();

            RaiseCamera();
    }
    private void RotateCamera()
    {
        Vector3 tempRot = transform.localEulerAngles;
        tempRot.y += Input.GetAxis(_input.CamHorizontal) * _speed;
        transform.localEulerAngles = tempRot;

        Vector3 rotationCamPivot = _camPivot.transform.localEulerAngles;
        rotationCamPivot.x += Input.GetAxis(_input.CamVertical) * -_speed;
        rotationCamPivot.x = PlayerBehaviour.ClampAngle(rotationCamPivot.x, -10, 50);
        _camPivot.localEulerAngles = rotationCamPivot;
    }
    public void RaiseCamera()
    {
        //Gets called when player is moving with pickup | Pushing Box
        PlayerBehaviour.States _currState = _playerBH.State;
        if(RaiseCam && _currState == PlayerBehaviour.States.PushingBox || _currState == PlayerBehaviour.States.WalkingWithPickup)
        {
            _camPivot.position = Vector3.Lerp(_camPivot.position, this.gameObject.transform.position + (Vector3.up * 2), Time.deltaTime);
        }
        else if(!RaiseCam)
        {
            _camPivot.position = Vector3.Lerp(_camPivot.position, _originalCameraPosition.position, Time.deltaTime);
        }
    }
}
