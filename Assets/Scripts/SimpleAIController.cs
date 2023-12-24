using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleAIController : Controller
{
    private NavMeshAgent _agent;
    
    private GameObject _target;
    
    private void Awake()
    {
        CharacterManager = GetComponent<CharacterManager>();    
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.angularSpeed = TurnSpeed/12;
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

    private void CheckIfCurrentTargetIsDead()
    {
        if (_target == null) return;
        if (_target.TryGetComponent(out CharacterManager characterManager))
        {
            if (characterManager.CharacterInfo.IsDead)
            {
                _target = null;
                _agent.ResetPath();
            }
        }
    }

    private void AttackTarget()
    {
        if (_target == null) return;
        if (Vector3.Distance(transform.position, _target.transform.position) <
            CharacterManager.CharacterInfo.EquippedWeapon.AttackRange)
        {
            Debug.Log(gameObject.name + " In Range");
            StartCoroutine(nameof(AttackDelay));      
        }
    }
    
    private IEnumerator AttackDelay()
    {
        if (IsInvoking(nameof(AttackDelay))) yield break;
        Debug.Log(gameObject.name + " Delay");
        yield return new WaitForSeconds(0.5f);
        Debug.Log(gameObject.name + " Attack");
        DoAttack();  
    }

    private void MoveToTarget()
    {
        if (_target == null) return;
        _agent.SetDestination(_target.transform.position);
    }

    private void ScanNearbyAreaForTarget()
    {
        if (_target != null) return;
        var results = new Collider[1024];
        var myPosition = transform.position;
        var radius = 10;
        var size = Physics.OverlapSphereNonAlloc(myPosition, radius, results);
        for (var i = 0; i < size; i++)
        {
            var objCollider = results[i];
            if (objCollider.TryGetComponent(out CharacterManager characterManager))
            {
                if (characterManager.CharacterInfo.IsDead) continue;
                if (characterManager.CharacterInfo.Side == CharacterManager.CharacterInfo.Side) continue;
                _target = objCollider.gameObject;
                // Create a ray from the character to target, and see if there is an obstacle between them
                var ray = new Ray(transform.position, _target.transform.position - myPosition);
                if (Physics.Raycast(ray, out var hit, radius))
                {
                    if (hit.collider.gameObject != _target)
                    {
                        _target = null;
                        continue;
                    }
                }
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
            CharacterManager.CharacterInfo.IsRunning = true;
            CharacterManager.CharacterInfo.CurrentSpeed = CharacterManager.CharacterInfo.DefaultSpeed;
            CharacterManager.AnimationController.SetMoveSpeed(CharacterManager.CharacterInfo.CurrentSpeed);
            CharacterManager.AnimationController.DoRunAnimation();
        }
        else
        {
            CharacterManager.CharacterInfo.IsRunning = false;
            CharacterManager.CharacterInfo.CurrentSpeed = 0;
            CharacterManager.AnimationController.SetMoveSpeed(CharacterManager.CharacterInfo.CurrentSpeed);
            CharacterManager.AnimationController.StopRunAnimation();
        }
        _agent.speed = CharacterManager.CharacterInfo.CurrentSpeed;
    }
}
