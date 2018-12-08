using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithMouse : MonoBehaviour {

    public Transform _camPivot;

    private float _speed = 2.0f;
	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 tempRot = transform.localEulerAngles;
        tempRot.y += Input.GetAxis("Mouse X") * _speed;
        transform.localEulerAngles = tempRot;

        Vector3 rotationCamePivot =_camPivot.transform.localEulerAngles;
        rotationCamePivot.x += Input.GetAxis("Mouse Y") * -_speed;
        rotationCamePivot.x = BasePlayerScript.ClampAngle(rotationCamePivot.x, -10, 50);
        _camPivot.localEulerAngles = rotationCamePivot;

        
    }
}
