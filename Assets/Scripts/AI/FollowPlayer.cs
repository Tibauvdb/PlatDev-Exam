using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour {
    private Transform _player;
    private BaseAIBehaviour _aiBehaviour;
    private int _visionDistance = 5; //5m

    private int _layerMask = 1 << 11;
	void Start () {
        //Get player from scene
        _player = GameObject.Find("Player").transform;
        _aiBehaviour = this.gameObject.GetComponent<BaseAIBehaviour>();

        _layerMask = ~_layerMask;
	}
	
	void Update () {
        //First check if player is in visionDistance
		if(Vector3.Distance(this.gameObject.transform.position,_player.position) <= _visionDistance)
        {

            //If Player is in VisionDistance -> Check if nothing is blocking LOS
            RaycastHit hit;
            Debug.DrawLine(this.gameObject.transform.position, _player.position, Color.red);
            if(Physics.Linecast(this.gameObject.transform.position,_player.position,out hit,_layerMask))
            {
                //If the lineCast returns true, something is in between the player & AI
                if (_aiBehaviour.IsAIFollowing)
                {
                    Debug.Log("LineCast Succes | Resetting IsAiFollowing");
                _aiBehaviour.IsAIFollowing = false;
                //Reset Agent
                _aiBehaviour.ResetAgent();
                }
            }
            else
            {
                _aiBehaviour.IsAIFollowing = true;
                _aiBehaviour.PlayerPosition = _player.position;
            }            
            
        }
        else if(_aiBehaviour.IsAIFollowing)
        {
            _aiBehaviour.IsAIFollowing = false;
        }
	}
}
