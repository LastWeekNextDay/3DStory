using System.Collections.Generic;

public class Sequence : Node
{
    public Sequence() : base()
    {
    }

    public Sequence(List<Node> children) : base(children)
    {
    }
    
    public override NodeState Evaluate()
    {
        bool anyChildRunning = false;

        for (int i = 0; i < Children.Count; i++)
        {
            Node child = Children[i];
            switch (child.Evaluate())
            {
                case NodeState.FAILURE:
                    State = NodeState.FAILURE;
                    return State;
                case NodeState.SUCCESS:
                    continue;
                case NodeState.RUNNING:
                    anyChildRunning = true;
                    continue;
                default:
                    State = NodeState.SUCCESS;
                    return State;
            }
        }

        State = anyChildRunning ? NodeState.RUNNING : NodeState.SUCCESS;
        return State;
    }
}
