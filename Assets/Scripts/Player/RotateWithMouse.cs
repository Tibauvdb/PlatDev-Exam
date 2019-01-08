using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithMouse : MonoBehaviour {
    private InputManager _input;
    public Transform _camPivot;

    private float _speed = 2.0f;
	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        _input = GameObject.Find("GameManager").GetComponent<InputManager>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 tempRot = transform.localEulerAngles;
        tempRot.y += Input.GetAxis(_input.CamHorizontal) * _speed;
        transform.localEulerAngles = tempRot;

        Vector3 rotationCamPivot =_camPivot.transform.localEulerAngles;
        rotationCamPivot.x += Input.GetAxis(_input.CamVertical) * -_speed;
        rotationCamPivot.x = PlayerBehaviour.ClampAngle(rotationCamPivot.x, -10, 50);
        _camPivot.localEulerAngles = rotationCamPivot;

        
    }
}
