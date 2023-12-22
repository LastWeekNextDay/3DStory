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
            }
        }
    }

    private void AttackTarget()
    {
        if (_target == null) return;
        if (Vector3.Distance(transform.position, _target.transform.position) >
            CharacterManager.CharacterInfo.EquippedWeapon.AttackRange) return;
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
        var size = Physics.OverlapSphereNonAlloc(transform.position, 10, results);
        for (var i = 0; i < size; i++)
        {
            var objCollider = results[i];
            if (objCollider.TryGetComponent(out CharacterManager characterManager))
            {
                if (characterManager.CharacterInfo.IsDead) continue;
                if (characterManager.CharacterInfo.Side == CharacterManager.CharacterInfo.Side) continue;
                _target = objCollider.gameObject;
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
        _agent.speed = CharacterManager.CharacterInfo.CurrentSpeed;
        if (_agent.remainingDistance > _agent.stoppingDistance)
        {
            CharacterManager.CharacterInfo.IsRunning = true;
            CharacterManager.AnimationController.SetMoveSpeed(_agent.speed);
            CharacterManager.AnimationController.DoRunAnimation();
        }
        else
        {
            CharacterManager.CharacterInfo.IsRunning = false;
            CharacterManager.AnimationController.StopRunAnimation();
        }
    }
}
