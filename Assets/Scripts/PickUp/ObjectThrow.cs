using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class ObjectThrow : MonoBehaviour {


    private AllowPickup _allowPickup;
    private LineRenderer _line;

    private float _v0 = 10; //Start Velocity
    private float _dis; // Distance
    private float _dStep;   //Distance Current Step
    [SerializeField]
    public float ThrowAngle = 25; //Angle at which the object will be thrown | Degrees
    private float _startAngle = 25;
    private float _g; //Gravity

    [SerializeField]
    private int _maxStep = 100;
    [SerializeField]
    private int _currentStep = 1;

    //private Vector3 _originalPosition;

    private float _newX;
    private float _newY;
    private float _newZ;

    private bool _allowCollision = true;
    private bool _colliding = false;

    //private Transform _parentTransform;
    private Vector3 _parentPos;
    private Quaternion _parentRot;
    public bool Throwing = false;


    void Start () {

        _allowPickup = this.gameObject.GetComponent<AllowPickup>();
        _line = this.gameObject.GetComponent<LineRenderer>();
        _line.enabled = true;

        //Set _g to downwards gravity in scene | 9.81 [m/s2]
        _g = -Physics.gravity.y;

        _parentPos = GameObject.Find("CamPivot").transform.position;
        _parentRot = GameObject.Find("CamPivot").transform.localRotation;

        CalculateDistance();

        #region Dependencies
        //dependency error if charactercontroller is not attached
#if DEBUG
        Assert.IsNotNull(_line, "DEPENDENCY ERROR: LineRenderer is missing from ObjectThrow Script");
#endif
        #endregion
    }

    void Update () {
        if (Throwing == false)
            DrawParabola();       
        
        if(this.gameObject.transform.parent != null)
        {
            _parentPos = GameObject.Find("Player").transform.position;
            _parentRot = GameObject.Find("Player").transform.localRotation;
        }

        CalculateDistance();

        if(Throwing == true && _colliding==false)
        {
            Throw();
            AddSteps();
            _allowCollision = true;
            _line.enabled = false;
        }
	}
    private void OnCollisionEnter(Collision collision)
    {
        if(_allowCollision == true && _currentStep >10)
        {
            //if Object collides with something - Stop movement
            _colliding = true;
            ResetThrow();
            _allowPickup.Thrown = false;
            _allowPickup.enabled = true;
            this.gameObject.GetComponent<ObjectThrow>().enabled = false;
        }
    }


    //Calculate distance | this will change when _v0 (Throwing power) get increased
    private void CalculateDistance()
    {
        _dis = (Mathf.Pow(_v0, 2) * Mathf.Sin(2 * (ThrowAngle * Mathf.PI / 180))) / _g; //x = (v0^2 * sin(2*alpha)) / g
    }

    //Update the position of the cube according to the parabola
    public void Throw()
    {
        ////Set new position
        this.gameObject.transform.position = StepPosCalculation(_currentStep);
    }

    //Add step so next update cube will take next position
    private void AddSteps()
    {
        //Add 1 to current steps      
        _currentStep++;   
    }

    //Draw the parabola that will visualize where cube would land
    private void DrawParabola()
    {
        _line.positionCount = _maxStep+(_maxStep/2);
        for (int i = 0; i < _line.positionCount; i++)
        {
            _line.SetPosition(i, StepPosCalculation(i));
        }
    }

    private Vector3 StepPosCalculation(int step)
    {
        float dStep = step * (_dis / _maxStep);

        ////Update Y Position | y = x * tan(alpha) - ( g / 2*v0^2*cos^2(alpha) ) * x^2 |
        _newY = _parentPos.y + (dStep * Mathf.Tan(ThrowAngle * (Mathf.PI / 180))) - (_g / (2 * (Mathf.Pow(_v0, 2)) * Mathf.Pow(Mathf.Cos(ThrowAngle * (Mathf.PI / 180)), 2))) * (Mathf.Pow(dStep, 2));

        _newX = _parentPos.x + dStep * (Mathf.Sin(_parentRot.eulerAngles.y * (Mathf.PI / 180)));

        _newZ = _parentPos.z + dStep * (Mathf.Cos(_parentRot.eulerAngles.y * (Mathf.PI / 180)));

        Vector3 newPos = new Vector3(_newX, _newY+1f, _newZ);

        return newPos;
    }
  

    private void ResetThrow()
    {
        _currentStep = 5;
        _line.positionCount = 0;
        _line.enabled = true;
        Throwing = false;
        _allowCollision = false;
        _colliding = false;
        ThrowAngle = _startAngle;
    }
}
