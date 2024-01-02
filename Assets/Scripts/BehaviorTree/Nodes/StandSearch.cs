using UnityEngine;
using UnityEngine.AI;

public class StandSearch : Node
{
    private Controller _controller;
    private float _searchRadius;
    public StandSearch(Controller controller, float viewDistance) : base()
    {
        _controller = controller;
        _searchRadius = viewDistance;
    }

    public override NodeState Evaluate()
    {
        if (_controller.TargetToAttack != null)
        {
            State = NodeState.SUCCESS;
            return State;
        }
        var results = new Collider[1024];
        var myPosition = _controller.transform.position + 1f * Vector3.up;
        var size = Physics.OverlapSphereNonAlloc(myPosition, _searchRadius, results);
        for (var i = 0; i < size; i++)
        {
            var objCollider = results[i];
            if (objCollider.TryGetComponent(out CharacterManager characterManager))
            {
                var ray = new Ray(myPosition, objCollider.transform.position + 1f * Vector3.up - myPosition);
                if (Physics.Raycast(ray, out var hit, _searchRadius))
                {
                    if (hit.collider.gameObject != objCollider.gameObject) continue;
                }
                if (characterManager.CharacterInfo.IsDead) continue;
                if (characterManager.CharacterInfo.side == _controller.CharacterManager.CharacterInfo.side) continue;
                _controller.TargetToAttack = characterManager.gameObject;
                State = NodeState.SUCCESS;
                return State;
            }
        }
        _controller.TargetToAttack = null;
        _controller.StopMovement();
        State = NodeState.FAILURE;
        return State;
    }
}
