using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockedOutCheck : MonoBehaviour {
    private PlayerBehaviour _playerBh;
	// Use this for initialization
	void Start () {
        _playerBh = this.gameObject.transform.parent.GetComponent<PlayerBehaviour>();
	}

    private void OnTriggerEnter(Collider other)
    {
        //if AI enters
        if(other.name.Contains("Type01") && other.gameObject.GetComponent<BaseAIBehaviour>().IsAIKnockedOut == false && _playerBh.State != PlayerBehaviour.States.Sitting)
        {
            _playerBh.KnockedOut();
        }
    }
}
