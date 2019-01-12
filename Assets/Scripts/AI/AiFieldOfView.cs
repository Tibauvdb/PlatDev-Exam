using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiFieldOfView : MonoBehaviour {
    public GameObject Player;
    private BaseAIBehaviour _aiBehaviour;

    private int _visionDistance = 5; //5m radius aroundAI
    private int _layerMask = 1 << 11; //Ignore pickups layer

    private PlayerBehaviour.States _currentState;
    private void Start()
    {
        _aiBehaviour = this.gameObject.GetComponent<BaseAIBehaviour>();
        _layerMask = ~_layerMask;
    }


    public bool CheckFieldOfView(bool allowReset)
    {
        //Check if player is in FOV
        if (Vector3.Distance(this.gameObject.transform.position, Player.transform.position) <= _visionDistance)
        {
            _currentState = Player.GetComponent<PlayerBehaviour>().State;

            RaycastHit hit;
            if (Physics.Linecast(this.gameObject.transform.position, Player.transform.position, out hit, _layerMask) || _currentState == PlayerBehaviour.States.Sitting || _currentState == PlayerBehaviour.States.KnockedOut)
            {
                //Something in between player & AI
                if (allowReset && _aiBehaviour.IsAIFollowing)
                    _aiBehaviour.ResetAgent();
                return false;
            }
            else
            {
                _aiBehaviour.PlayerPosition = Player.transform.position;
                return true;

            }
        }
        else
            return false;
    }
}
