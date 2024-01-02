using System.Collections.Generic;
using UnityEngine;

public class SimpleAIBT : BehaviorTree
{
    [SerializeField] private Controller _controller;
    [SerializeField] private float _searchRadius;
    protected override Node SetupTree()
    {
        Node root = new Sequence(new List<Node>{
            new Alive(_controller.CharacterManager.CharacterInfo),
            new StandSearch(_controller, _searchRadius),
            new RushAttack(_controller, _searchRadius)
        });

        return root;
    }
}
