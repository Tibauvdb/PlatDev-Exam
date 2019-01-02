using System.Collections.Generic;

public abstract class CompositeNode : INode
{
    protected INode[] _nodes;

    public CompositeNode(params INode[] nodes)
    {
        _nodes = nodes;
    }

    public abstract IEnumerator<NodeResult> Tick();
}