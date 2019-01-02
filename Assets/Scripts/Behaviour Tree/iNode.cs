using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface INode
{
    //declareert interface
    IEnumerator<NodeResult> Tick();
    //NodeResult Tick();
}
