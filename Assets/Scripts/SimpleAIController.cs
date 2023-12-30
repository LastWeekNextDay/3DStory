using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SimpleAIController : Controller
{
    private NavMeshAgent _agent;
    
    [SerializeField] private GameObject target;

    private bool _bufferingAttack;
    
    private const int SearchRadius = 10;
    
    protected override void Awake()
    {
        base.Awake();  
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.angularSpeed = TurnSpeed/12;
        CharacterManager.CharacterInfo.OnDeath += DisableAI;
    }

    private void Update()
    {
        CheckIfCurrentTargetIsDead();
        ScanNearbyAreaForTarget();
        MoveToTarget();
        LookRotation();
        MoveAnimation();
        AttackTarget();
    }

    private void DisableAI()
    {
        target = null;
        _agent.ResetPath();
        enabled = false;
    }

    private void CheckIfCurrentTargetIsDead()
    {
        if (target == null) return;
        if (target.TryGetComponent(out CharacterManager characterManager))
        {
            if (characterManager.CharacterInfo.IsDead)
            {
                target = null;
                _agent.ResetPath();
            }
        }
    }
    
    private void AttackTarget()
    {
        if (target == null) return;
        if (Vector3.Distance(transform.position, target.transform.position) <
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
        DoAttack();
        _bufferingAttack = false;
    }

    private void MoveToTarget()
    {
        if (target == null) return;
        _agent.SetDestination(target.transform.position);
    }

    private void ScanNearbyAreaForTarget()
    {
        if (target != null) return;
        var results = new Collider[1024];
        var myPosition = transform.position + 1f * Vector3.up;
        var size = Physics.OverlapSphereNonAlloc(myPosition, SearchRadius, results);
        for (var i = 0; i < size; i++)
        {
            var objCollider = results[i];
            if (objCollider.TryGetComponent(out CharacterManager characterManager))
            {
                var ray = new Ray(myPosition, objCollider.transform.position + 1f * Vector3.up - myPosition);
                if (Physics.Raycast(ray, out var hit, SearchRadius))
                {
                    if (hit.collider.gameObject != objCollider.gameObject) continue;
                }
                if (characterManager.CharacterInfo.IsDead) continue;
                if (characterManager.CharacterInfo.side == CharacterManager.CharacterInfo.side) continue;
                target = objCollider.gameObject;
                return;
            }
        }
    }

    private void LookRotation()
    {
        if (_agent.velocity == Vector3.zero) return;
        var lookRotation = Quaternion.LookRotation(_agent.velocity);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _agent.angularSpeed);
    }

    private void MoveAnimation()
    {
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
}
