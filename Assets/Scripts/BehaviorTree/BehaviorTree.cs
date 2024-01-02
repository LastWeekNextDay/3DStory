using UnityEngine;

public abstract class BehaviorTree : MonoBehaviour
{
    private Node _root;   

    protected void Start(){
        _root = SetupTree();
    }

    protected void Update(){
        _root?.Evaluate();
    }

    protected abstract Node SetupTree();
}
