using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStateMachineBehaviour : StateMachineBehaviour,ISetBehaviour<PlayerBehaviour,BaseAIBehaviour,Animator> {

    //Get playerBehaviour Script
    public PlayerBehaviour SetPlayerBehaviour(PlayerBehaviour PlayerBehaviourScript)
    {
        PlayerBehaviourScript = GameObject.Find("Player").GetComponent<PlayerBehaviour>();
        return PlayerBehaviourScript;
    }

    //Get BaseAiBehaviour Script
    public BaseAIBehaviour SetAIBehaviour(BaseAIBehaviour AiBehaviourScript,Animator animator)
    {
        AiBehaviourScript = animator.gameObject.transform.parent.gameObject.GetComponent<BaseAIBehaviour>();
        return AiBehaviourScript;
    }
}
