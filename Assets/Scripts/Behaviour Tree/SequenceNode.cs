using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class SequenceNode : CompositeNode
{

    public SequenceNode(params INode[] nodes) : base(nodes)
    {
    }

    public override IEnumerator<NodeResult> Tick()
    {
        NodeResult returnNodeResult = NodeResult.Succes;

        foreach(INode node in _nodes)
        {
            IEnumerator<NodeResult> result = node.Tick();

            while (result.MoveNext() && result.Current == NodeResult.Running)
            {
                yield return NodeResult.Running;
            }

            returnNodeResult = result.Current;

            if (returnNodeResult == NodeResult.Succes)
                continue;

            if (returnNodeResult == NodeResult.Failure)
                break;
        }

        yield return returnNodeResult;
    }
}
