using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetStateMachineBehaviour : MonoBehaviour,ISetStateMachineBehaviour<StateMachineBehaviour>,ISetPlayerBehaviour<PlayerBehaviour> {
    public List<StateMachineBehaviour> _classes = new List<StateMachineBehaviour>();

    public void SetStateBehaviour(StateMachineBehaviour state)
    {
        _classes.Add(state);
    }
    public void SetPlayerBehaviour(PlayerBehaviour bps)
    {
        foreach(StateMachineBehaviour smb in _classes)
        {
            SetPlayerBehaviour(bps);
        }
    }
}
