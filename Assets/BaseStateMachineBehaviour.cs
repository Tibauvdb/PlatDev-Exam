using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStateMachineBehaviour : StateMachineBehaviour {

    public void SetPlayerBehaviour(PlayerBehaviour _pbs)
    {
        Debug.Log("Happening");
        _pbs = GameObject.Find("Player").GetComponent<PlayerBehaviour>();
    }

}
