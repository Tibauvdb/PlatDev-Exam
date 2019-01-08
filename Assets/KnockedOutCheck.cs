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
        if(other.tag == "AI")
        {
            Debug.Log("AI enters");
            _playerBh.KnockedOut();
        }
    }
}
