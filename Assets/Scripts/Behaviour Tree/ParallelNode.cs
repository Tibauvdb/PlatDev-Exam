using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//Loopt over verschillende nodes heen en kan via een policy
//zeggen wat er moet gebeuren
public class ParallelNode : CompositeNode
{
    //public delegate NodeResult Policy(NodeResult result);

    public delegate ParallelNodePolicyAccumulator Policy();
    private Policy _policy;


    public ParallelNode(Policy policy, params INode[] nodes) : base(nodes)
    {
        _policy = policy;        }

    int count = 0;

    public override IEnumerator<NodeResult> Tick()
    {
        
        ParallelNodePolicyAccumulator acc = _policy();
        NodeResult returnNodeResult = NodeResult.Failure;

        foreach(INode node in _nodes)
        {
            IEnumerator<NodeResult> result = node.Tick();

            while(result.MoveNext() && result.Current == NodeResult.Running)

            returnNodeResult = acc.Policy(result.Current);
        }

        yield return returnNodeResult;
    }

    //public override NodeResult Tick()
    //{
    //    NodeResult result = NodeResult.Failure;
    //    foreach(INode node in _nodes)
    //    {
    //        if(result == NodeResult.Failure)
    //        {
    //            count++;
    //        }
    //    }
    //    if (count >= 4)
    //    {
    //        result = NodeResult.Failure;
    //    }
    //    else
    //        result = NodeResult.Succes;
    //    return result;
    //}
}
