using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseAIBehaviour))]
public class AiFieldOfView : MonoBehaviour {   
    private BaseAIBehaviour _aiBehaviour;

    private GameObject _player;

    private float FieldOfView = 120; //Field of View Cone
    private int _visionDistance = 5; //5m radius aroundAI
    private int _layerMask = 1 << 11; //Ignore pickups layer

    private PlayerBehaviour.States _currentState;

    private void Start()
    {
        SetFields();
        _layerMask = ~_layerMask;
    }


    public bool CheckFieldOfView(bool allowReset)
    {
        //Check if player is in FOV
        SetFields();

        //Check if player is in Vision Distance
        if (Vector3.Distance(this.gameObject.transform.position, _player.transform.position) <= _visionDistance)
        {
            //Check if Player is in Field of View Cone (90 Degrees from forward)
            Vector3 directionToPlayer = _player.transform.position - this.gameObject.transform.position;
            if (Quaternion.Angle(this.gameObject.transform.rotation, Quaternion.LookRotation(directionToPlayer)) < (FieldOfView / 2))
            {
                return CheckLineOfSight(allowReset);
            }
            return false;
        }
        else
            return false;
    }

    private bool CheckLineOfSight(bool allowReset)
    {
        _currentState = _player.GetComponent<PlayerBehaviour>().State;
        RaycastHit hit;
        if (Physics.Linecast(this.gameObject.transform.position, _player.transform.position, out hit, _layerMask) || _currentState == PlayerBehaviour.States.Sitting || _currentState == PlayerBehaviour.States.KnockedOut)
        {
            //Something in between player & AI
            if (allowReset && _aiBehaviour.IsAIFollowing)
                _aiBehaviour.ResetAgent();
            return false;
        }
        else
        {
            _aiBehaviour.PlayerPosition = _player.transform.position;
            return true;
        }
    }

    private void SetFields()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _aiBehaviour = this.gameObject.GetComponent<BaseAIBehaviour>();
    }

    public bool CheckBoxPushing()
    {        
        if( _player.GetComponent<PlayerBehaviour>().State == PlayerBehaviour.States.PushingBox)
        {
            Debug.Log("BoxPushing True");
            return true;
        }
        return false;
    }
}
