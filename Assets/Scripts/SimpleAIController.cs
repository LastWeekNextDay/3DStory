using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SimpleAIController : Controller
{
    [SerializeField] private NavMeshAgent _agent;
    private bool _bufferingAttack;
    protected override void Awake()
    {
        base.Awake();  
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.angularSpeed = TurnSpeed/12;
        CharacterManager.CharacterInfo.OnDeath += DisableAI;
    }

    private void DisableAI()
    {
        TargetToMoveTo = null;
        StopMovement();
        enabled = false;
    }

    public override void DoAttack()
    {
        AttackTarget();
    }

    private void AttackTarget()
    {
        if (TargetToAttack == null) return;
        if (Vector3.Distance(transform.position, TargetToAttack.transform.position) <
            CharacterManager.CharacterInfo.EquippedWeapon.AttackRange)
        {
            StartCoroutine(nameof(AttackDelay));      
        }
    }

    private IEnumerator AttackDelay()
    {
        if (_bufferingAttack) yield break;
        _bufferingAttack = true;
       yield return new WaitForSeconds(0.3f);
        DoGenericAttack();
        _bufferingAttack = false;
    }

    protected override void LookUpdate()
    {
        Quaternion lookRotation;
        Vector3 dir;
        if (TargetToAttack != null)
        {
            var myPositionAdj = transform.position + 1f * Vector3.up;
            var targetPositionAdj = TargetToAttack.transform.position + 1f * Vector3.up;
            dir = targetPositionAdj - myPositionAdj;
            var ray = new Ray(myPositionAdj, dir);
            if (Physics.Raycast(ray, out var hit, 10000))
            {
                if (hit.collider.gameObject != TargetToAttack)
                {
                    if (_agent.velocity == Vector3.zero){
                        dir = transform.forward;
                    } else {
                        dir = _agent.velocity;
                    }
                }
            }
        }
        else
        {
            if (_agent.velocity == Vector3.zero){
                dir = transform.forward;
            } else {
                dir = _agent.velocity;
            }
        }
        lookRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 360f/TurnSpeed);
    }

    protected override void MoveUpdate()
    {
        if (TargetToMoveTo == null) return;
        _agent.SetDestination(TargetToMoveTo.transform.position);
        if (_agent.remainingDistance > _agent.stoppingDistance)
        {
            CharacterManager.CharacterInfo.RealSpeed = CharacterManager.CharacterInfo.logicalSpeed;
            CharacterManager.AnimationController.SetMoveSpeed(CharacterManager.CharacterInfo.RealSpeed);
            if (CharacterManager.CharacterInfo.IsRunning) return;
            CharacterManager.CharacterInfo.IsRunning = true;
            CharacterManager.AnimationController.DoRunAnimation();
        }
        else
        {
            CharacterManager.CharacterInfo.RealSpeed = 0;
            CharacterManager.AnimationController.SetMoveSpeed(CharacterManager.CharacterInfo.RealSpeed);
            if (!CharacterManager.CharacterInfo.IsRunning) return;
            CharacterManager.CharacterInfo.IsRunning = false;
            CharacterManager.AnimationController.StopRunAnimation();
        }
        _agent.speed = CharacterManager.CharacterInfo.RealSpeed;
    }

    public override void StopMovement()
    {
        _agent.ResetPath();
    }
}
