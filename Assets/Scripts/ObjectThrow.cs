using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class ObjectThrow : MonoBehaviour {

    private LineRenderer _line;

    [SerializeField]
    private float _v0 = 10; //Start Velocity
    private float _dis; // Distance
    private float _dStep;   //Distance Current Step
    private float _angle = 45; //Angle at which the object will be thrown | Degrees
    private float _g; //Gravity

    [SerializeField]
    private int _maxStep = 100;
    [SerializeField]
    private int _currentStep = 1;

    private Vector3 _originalPosition;

    private float _newX;
    private float _newY;
    private float _newZ;

    private bool _allowCollision = true;
    private bool _colliding = false;

    //public Canvas Canvas;
    void Start () {
        //Get Linerenderer on Cube (move to Player?)
        _line = this.gameObject.GetComponent<LineRenderer>();
        _line.startWidth = 0.2f;
        _line.endWidth = 0.2f;

        //Set _g to downwards gravity in scene | 9.81 [m/s2]
        _g = -Physics.gravity.y;

        //Get the original position of the cube
        _originalPosition = this.gameObject.transform.position;

        CalculateDistance();

        #region Dependencies
        //dependency error if charactercontroller is not attached
#if DEBUG
        Assert.IsNotNull(_line, "DEPENDENCY ERROR: LineRenderer is missing from ObjectThrow Script");
#endif
        #endregion
    }

    void Update () {

        //As long as the object isn't colliding with anything, continue the parabole track
        if(_colliding==false)
        {
        CalculateDistance();
        UpdatePosition();
        AddSteps();
        DrawParabola();
        }
	}
    private void OnCollisionEnter(Collision collision)
    {
        if(_allowCollision == true && _currentStep >1)
        {
        //if Object collides with something - Stop movement
        _colliding = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //Allow object to stop after exiting it's first collision (usually player or floor it starts the parabole from)
        _allowCollision = true;
    }

    //Calculate distance | this will change when _v0 (Throwing power) get increased
    private void CalculateDistance()
    {
        _dis = (Mathf.Pow(_v0, 2) * Mathf.Sin(2 * (_angle * Mathf.PI / 180))) / _g; //x = (v0^2 * sin(2*alpha)) / g
    }

    //Update the position of the cube according to the parabola
    private void UpdatePosition()
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
        _newY = _originalPosition.y + (dStep * Mathf.Tan(_angle * (Mathf.PI / 180))) - (_g / (2 * (Mathf.Pow(_v0, 2)) * Mathf.Pow(Mathf.Cos(_angle * (Mathf.PI / 180)), 2))) * (Mathf.Pow(dStep, 2));

        _newX = _originalPosition.x + dStep * (Mathf.Cos(this.gameObject.transform.rotation.eulerAngles.y * (Mathf.PI / 180)));

        _newZ = _originalPosition.z + dStep * (Mathf.Sin(this.gameObject.transform.rotation.eulerAngles.y * (Mathf.PI / 180)));

        Vector3 newPos = new Vector3(_newX, _newY+1f, _newZ);

        return newPos;
    }
  
    /* private void CheckFinalPosition(RaycastHit hitinfo)
    {
        for (int steps = _maxStep; steps < _maxStep * 2; steps++)
        {
            //Debug.Log(hitinfo.point.y);
            if (hitinfo.point.y >= StepPosCalculation(steps).y)
            {
                //This is the step+1 where the box will hit the ground
                //Set canvas element position to this position
                Canvas.transform.position = StepPosCalculation(steps-1);
                return;
            }
        }
    }*/
   
    /*private void CheckObjectCollision()
    {
        //Check if the object is colliding with the ground at _MaxSteps
        RaycastHit hitinfo;

        //Get position at the last step 
        #region WrittenOutLastStepPos

        #endregion

        Vector3 lastStepPos = StepPosCalculation(_maxStep);

        //If this raycast does not return true, the object will land on the ground
        if (Physics.Raycast(lastStepPos,Vector3.down,out hitinfo)){
            //Check if there is a floor below (also trigger when inside object)
            if (lastStepPos.y >= hitinfo.point.y)
            {          
                CheckFinalPosition(hitinfo);
                
                #region FirstIdea
                    /* 
                     //Option 2 - The cube will need to fall further
                     //Y position is known - calculate X/Z Position
                     float yEnd = hitinfo.point.y - lastStepPos.y;
                     float tanAlpha = (Mathf.Tan(_angle * (Mathf.PI / 180)));
                     float xTop = Mathf.Sqrt(Mathf.Pow(tanAlpha, 2) - (4 * yEnd) * (_g / (Mathf.Pow(_v0, 2) * Mathf.Pow(Mathf.Cos(_angle), 2))));
                     float xBot = 2 * (_g / (Mathf.Pow(_v0, 2) * Mathf.Pow(Mathf.Cos(_angle), 2)));

                     float x1 = ((tanAlpha + xTop) / xBot);
                     float x2 = ((tanAlpha - xTop) / xBot);
                     Debug.Log(_dis +"x1 = " + x1);
                     Debug.Log(_dis + "x2 = " + x2);
                     if (x1 >= 0)
                     {
                         //x1 is right x
                         float uiX = x1 * (Mathf.Cos(this.gameObject.transform.rotation.eulerAngles.y * (Mathf.PI / 180)));
                         float uiZ = x1 * (Mathf.Sin(this.gameObject.transform.rotation.eulerAngles.y * (Mathf.PI / 180)));
                         Canvas.transform.position =  new Vector3(uiX,hitinfo.transform.position.y+0.5f,uiZ);
                     }
                     else
                     {
                         //x2 is right x
                         float uiX = _originalPosition.x + x2 * (Mathf.Cos(this.gameObject.transform.rotation.eulerAngles.y * (Mathf.PI / 180)));
                         float uiZ = _originalPosition.z + x2 * (Mathf.Sin(this.gameObject.transform.rotation.eulerAngles.y * (Mathf.PI / 180)));
                         Canvas.transform.position = new Vector3(uiX, hitinfo.point.y + 0.5f, uiZ);
                     }

                    #endregion
            }
         
        }
    }*/
}
