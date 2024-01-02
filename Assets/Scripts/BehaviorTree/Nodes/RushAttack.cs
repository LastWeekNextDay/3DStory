using UnityEngine;
using UnityEngine.AI;

public class RushAttack : Node
{
    private Controller _controller;
    private float _viewDistance;

    public RushAttack(Controller controller, float viewDistance) : base()
    {
        _controller = controller;
        _viewDistance = viewDistance;
    }

    public override NodeState Evaluate()
    {
        if (_controller.CharacterManager.CharacterInfo.IsDead)
        {
            State = NodeState.FAILURE;
            return State;
        }
        if (_controller.TargetToAttack == null)
        {
            State = NodeState.FAILURE;
            return State;
        }
        var myPositionAdj = _controller.transform.position + 1f * Vector3.up;
        var targetPositionAdj = _controller.TargetToAttack.transform.position + 1f * Vector3.up;
        var ray = new Ray(myPositionAdj, targetPositionAdj - myPositionAdj);
        if (Physics.Raycast(ray, out var hit, _viewDistance))
        {
            if (hit.collider.gameObject == _controller.TargetToAttack.gameObject)
            {
                var movePosAdjustedWithWeaponRange = _controller.TargetToAttack.transform.position - 
                (_controller.CharacterManager.CharacterInfo.EquippedWeapon.AttackRange * 
                Vector3.Normalize(_controller.TargetToAttack.transform.position - _controller.transform.position));

                if (Vector3.Distance(_controller.transform.position, movePosAdjustedWithWeaponRange) > _controller.CharacterManager.CharacterInfo.EquippedWeapon.AttackRange)
                {
                    _controller.TargetToMoveTo.transform.position = movePosAdjustedWithWeaponRange;
                }
            }
        }
        _controller.DoAttack();
        State = NodeState.RUNNING;
        return State;
    }
}
