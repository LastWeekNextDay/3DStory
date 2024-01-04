using UnityEngine;

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
        if (_controller.TargetToAttack.GetComponent<CharacterManager>().CharacterInfo.IsDead)
        {
            _controller.TargetToAttack = null;
            State = NodeState.SUCCESS;
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
                if (Vector3.Distance(_controller.transform.position, _controller.TargetToAttack.transform.position) > _controller.CharacterManager.CharacterInfo.EquippedWeapon.AttackRange)
                {
                    var dir = _controller.TargetToAttack.transform.position - _controller.transform.position;
                    var dirBuffer = dir * 0.05f;
                    var movePosAdjByAttackRange = _controller.TargetToAttack.transform.position - dir.normalized * _controller.CharacterManager.CharacterInfo.EquippedWeapon.AttackRange + dirBuffer;
                    _controller.TargetToMoveTo.transform.position = movePosAdjByAttackRange;
                }
            }
        }
        _controller.DoAttack();
        State = NodeState.RUNNING;
        return State;
    }
}
