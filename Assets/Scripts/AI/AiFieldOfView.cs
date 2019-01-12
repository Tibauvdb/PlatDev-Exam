using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiFieldOfView : MonoBehaviour {
    private BaseAIBehaviour _aiBehaviour;
    private GameObject _player;

    private int _visionDistance = 5; //5m radius aroundAI
    private int _layerMask = 1 << 11; //Ignore pickups layer

    private PlayerBehaviour.States _currentState;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _aiBehaviour = this.gameObject.GetComponent<BaseAIBehaviour>();
        _layerMask = ~_layerMask;
    }


    public bool CheckFieldOfView(bool allowReset)
    {
        //Check if player is in FOV
        if (Vector3.Distance(this.gameObject.transform.position, _player.transform.position) <= _visionDistance)
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
        else
            return false;
    }
}
