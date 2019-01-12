using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStateMachineBehaviour : StateMachineBehaviour {

    public PlayerBehaviour SetPlayerBehaviour(PlayerBehaviour PlayerBehaviourScript)
    {
        Debug.Log("Happening");
        PlayerBehaviourScript = GameObject.Find("Player").GetComponent<PlayerBehaviour>();
        return PlayerBehaviourScript;
    }

}
