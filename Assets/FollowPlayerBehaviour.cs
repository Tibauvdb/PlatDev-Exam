using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerBehaviour : MonoBehaviour {
    private BaseAIBehaviour _aiBehaviour;
    private AiFieldOfView _aiFOV;

	// Use this for initialization
	void Start () {
        _aiBehaviour = this.gameObject.GetComponent<BaseAIBehaviour>();
        _aiFOV = this.gameObject.GetComponent<AiFieldOfView>();
	}
	
	// Update is called once per frame
	void Update () {
        _aiBehaviour.IsAIFollowing = _aiFOV.CheckFieldOfView(true);
	}


}
