using System.Collections.Generic;

public class Selector : Node
{
    public Selector() : base()
    {
    }

    public Selector(List<Node> children) : base(children)
    {
    }
    
    public override NodeState Evaluate()
    {
        for (int i = 0; i < Children.Count; i++)
        {
            Node child = Children[i];
            switch (child.Evaluate())
            {
                case NodeState.FAILURE:
                    continue;
                case NodeState.SUCCESS:
                    State = NodeState.SUCCESS;
                    return State;
                case NodeState.RUNNING:
                    State = NodeState.RUNNING;
                    return State;
                default:
                    continue;
            }
        }

        State = NodeState.FAILURE;
        return State;
    }
}
