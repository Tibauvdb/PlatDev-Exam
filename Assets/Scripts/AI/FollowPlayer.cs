using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//No Longer Used - Replaced by Field Of View Script
public class FollowPlayer : MonoBehaviour {

    private Transform _player;
    private BaseAIBehaviour _aiBehaviour;
    private int _visionDistance = 5; //5m

    //Make sure Linecast ignores the "Pickups" Layer
    private int _layerMask = 1 << 11;
    PlayerBehaviour.States _currState; 
    void Start () {
        //Get player from scene - 
        _player = GameObject.Find("Player").transform;
        _aiBehaviour = this.gameObject.GetComponent<BaseAIBehaviour>();

        _layerMask = ~_layerMask;
	}
	
	void Update () {
        #region Previous
        //First check if player is in visionDistance
        _currState = _player.gameObject.GetComponent<PlayerBehaviour>().State;

        if (Vector3.Distance(this.gameObject.transform.position,_player.position) <= _visionDistance)
        {
            //If Player is in VisionDistance -> Check if nothing is blocking LOS
            RaycastHit hit;
            if(Physics.Linecast(this.gameObject.transform.position,_player.position,out hit,_layerMask) || _currState == PlayerBehaviour.States.Sitting || _currState == PlayerBehaviour.States.KnockedOut)
            {
                //If the lineCast returns true, something is in between the player & AI
                if (this.gameObject.name.Contains("Type01"))
                    if(_aiBehaviour.IsAIFollowing)
                        _aiBehaviour.ResetAgent();
                    _aiBehaviour.IsAIFollowing = false;
                if (this.gameObject.name.Contains("Type02"))
                {
                    //Doesn't happen
                    _aiBehaviour.IsAILooking = false;;
                }
                //Reset Agent
                
            }
            else
            {
                //Check if following or looking type
                if(this.gameObject.name.Contains("Type01"))
                    _aiBehaviour.IsAIFollowing = true;
                if (this.gameObject.name.Contains("Type02"))
                    _aiBehaviour.IsAILooking = true;

                _aiBehaviour.PlayerPosition = _player.position;
            }              
        }
        else
        {
            if (this.gameObject.name.Contains("Type01"))
                _aiBehaviour.IsAIFollowing = false;
            if (this.gameObject.name.Contains("Type02"))
                _aiBehaviour.IsAILooking = false;
        }
        #endregion


    }
}
