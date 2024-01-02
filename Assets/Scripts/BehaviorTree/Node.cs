using System.Collections.Generic;

public enum NodeState
{
    SUCCESS,
    FAILURE,
    RUNNING
}

public class Node
{
    protected NodeState State;

    public Node Parent { get; protected set; }
    protected List<Node> Children = new List<Node>();

    private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

    public Node()
    {
        Parent = null;
    }

    public Node(List<Node> children)
    {
        Parent = null;
        for (int i = 0; i < children.Count; i++)
        {
            AttachChild(children[i]);
        }
    }

    private void AttachChild(Node child)
    {
        Children.Add(child);
        child.Parent = this;
    }

    public virtual NodeState Evaluate()
    {
        return NodeState.FAILURE;
    }

    public void SetData(string key, object value)
    {
        _dataContext[key] = value;
    }

    public object GetData(string key)
    {
        if (_dataContext.TryGetValue(key, out object value))
        {
            return value;
        }

        Node parent = Parent;
        while (parent != null)
        {
            value = parent.GetData(key);
            if (value != null)
            {
                return value;
            }
            parent = parent.Parent;
        }
        return null;
    }

    public bool ClearData(string key)
    {
        if (_dataContext.ContainsKey(key))
        {
            _dataContext.Remove(key);
            return true;
        }

        Node parent = Parent;
        while (parent != null)
        {
            bool cleared = parent.ClearData(key);
            if (cleared)
            {
                return true;
            }
            parent = parent.Parent;
        }
        return false;
    }
}
