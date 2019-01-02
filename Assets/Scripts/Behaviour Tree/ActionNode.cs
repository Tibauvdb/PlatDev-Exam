using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionNode : INode
{
    //readonly - een keer toekennen daarna niet meer veranderbaar

    public delegate IEnumerator<NodeResult> Action();
    private readonly Action _action;

    public ActionNode(Action action)
    {
        _action = action;
    }
    
    public IEnumerator<NodeResult> Tick()
    {
        return _action();
    }
}