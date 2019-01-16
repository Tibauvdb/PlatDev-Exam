using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Found in Base Mechanics - 2. Camera 
public class RotateWithMouse : MonoBehaviour {
    [SerializeField]
    public bool AllowRotation { get; set; }
    public bool RaiseCam { get; set; }

    public Transform _camPivot;
    private InputManager _input;

    private PlayerBehaviour _playerBH;
    private PlayerBehaviour.States _currState;
    private Transform _originalCameraPosition;

    private float _camRotationSpeed = 2.0f;
    private float _lerpValue = 0;
    private bool _isSitting = false;

    
    // Use this for initialization
    void Start () {
        Cursor.lockState = CursorLockMode.Locked;

        _input = GameObject.Find("GameManager").GetComponent<InputManager>();

        _playerBH = this.gameObject.GetComponent<PlayerBehaviour>();
        _originalCameraPosition = this.gameObject.transform.Find("KnockedOutCheck").transform;

        AllowRotation = true;
        RaiseCam = false;
    }
	
	// Update is called once per frame
	void Update () {
        _currState = _playerBH.State;

        if (AllowRotation)
            RotateCamera();

        RaiseCamera();
    }

    private void RotateCamera()
    {
        //Horizontal Camera rotation
        if (_currState != PlayerBehaviour.States.Sitting)
        {
            HorizontalRotation(transform); //if player isn't sitting down, use player object for horizontal rotation

            if (_isSitting && _currState == PlayerBehaviour.States.Walking)
            {
                StandingUpCamera();
            }
        }
        else //Player Is Sitting -> Allow camera to rotate freely
        {
            HorizontalRotation(_camPivot);

            _isSitting = true;
        }

        RotateVertical();
    }

    private void HorizontalRotation(Transform baseRotate)
    {
        Vector3 temprot;

        temprot = baseRotate.localEulerAngles;
        temprot.y += Input.GetAxis(_input.CamHorizontal) * _camRotationSpeed;

        baseRotate.localEulerAngles = temprot;
    }

    private void RaiseCamera()
    {
        if(RaiseCam && _currState == PlayerBehaviour.States.PushingBox || _currState == PlayerBehaviour.States.WalkingWithPickup)
        {
            _camPivot.position = Vector3.Lerp(_camPivot.position, this.gameObject.transform.position + (Vector3.up * 2), Time.deltaTime);
        }
        else if(!RaiseCam)
        {
            _camPivot.position = Vector3.Lerp(_camPivot.position, _originalCameraPosition.position, Time.deltaTime);
        }
    }

    private void StandingUpCamera()
    {
        Debug.Log("Standing Up");
        _lerpValue += Time.deltaTime / 2;

        Vector3 currAngle = new Vector3(
            Mathf.LerpAngle(_camPivot.transform.localRotation.x, _originalCameraPosition.transform.rotation.x + 35,_lerpValue),
            Mathf.LerpAngle(_camPivot.transform.localRotation.y, _originalCameraPosition.transform.rotation.y, _lerpValue),
            _camPivot.transform.rotation.z);

        _camPivot.transform.eulerAngles = currAngle;
        
        if (_lerpValue >= 1f)
        {
            _isSitting = false;
            _lerpValue = 0;
        }
    }

    private void RotateVertical()
    {
        Vector3 rotationCamPivot = _camPivot.transform.localEulerAngles;

        rotationCamPivot.x += Input.GetAxis(_input.CamVertical) * -_camRotationSpeed;
        rotationCamPivot.x = PlayerBehaviour.ClampAngle(rotationCamPivot.x, -10, 50);

        _camPivot.localEulerAngles = new Vector3(rotationCamPivot.x,_camPivot.localEulerAngles.y,_camPivot.localEulerAngles.z);
    }
}
