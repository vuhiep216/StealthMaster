using System;
using System.Collections;
using System.Collections.Generic;
using Funzilla;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private DynamicJoystick dynamicJoystick;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Character character;

    private const float WalkSpeed = 9.99f;
    private const float AttackRange = 3.0f;
    private const float AttackTime = 0.7f;
    private enum State
    {
        Idle,Walk,Run,Attack,Win,Die
    }

    private Enemy[] _enemies;
    private Exit _exit;

    private State _state = State.Idle;
    private float _attackTime;
    private float _hp;
    
    private void Start()
    {
        _enemies = FindObjectsOfType<Enemy>();
        _exit = FindObjectOfType<Exit>();
        _hp = 50;
    }

    private void Update()
    {
        var move = dynamicJoystick.Direction * speed;
        agent.velocity = new Vector3(move.x, 0, move.y);
        FindNearestEnemy();
        
        if (_state != State.Die && Vector3.Distance(transform.position,_exit.transform.position)<1.0f)
        {
            ChangeState(State.Win);
            return;
        }

        if (_hp <= 0)
        {
            ChangeState(State.Die);
        }

        switch (_state)
        {
            case State.Idle:
                if(TryAttack()) return;
                if (agent.velocity.magnitude > 0) ChangeState(State.Walk);
                break;
            case State.Walk:
                if(TryAttack()) return;
                if(agent.velocity.magnitude > WalkSpeed) ChangeState(State.Run);
                else if(agent.velocity.magnitude <= 0) ChangeState(State.Idle);
                break;
            case State.Run:
                if(TryAttack()) return;
                if(agent.velocity.magnitude<=WalkSpeed) ChangeState((State.Walk));
                break;
            case State.Attack:
                _attackTime -= Time.deltaTime;
                if (_attackTime <= 0)
                {
                    ChangeState(State.Walk);
                }
                break;
            case State.Win:
                break;
            case State.Die:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ChangeState(State newState)
    {
        if(newState==_state)return;
        ExitCurrentState();
        _state = newState;
        EnterNewState();
    }

    private void ExitCurrentState()
    {
        switch (_state)
        {
            case State.Idle:
                break;
            case State.Walk:
                break;
            case State.Run:
                break;
            case State.Attack:
                break;
            case State.Win:
                break;
            case State.Die:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void EnterNewState()
    {
        switch (_state)
        {
            case State.Idle:
                character.AnimateIdle();
                break;
            case State.Walk:
                character.AnimateWalk();
                break;
            case State.Run:
                character.AnimateRun();
                break;
            case State.Attack:
                character.AnimateAttack();
                _attackTime = AttackTime;
                break;
            case State.Win:
                character.AnimateWin();
                break;
            case State.Die:
                character.AnimateDie();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private bool TryAttack()
    {
        foreach (var e in _enemies)
        {
            if(Vector3.Distance( e.transform.position, transform.position) < AttackRange)
            {
                ChangeState(State.Attack);
                return true;
            }   
        }
        return false;
    }

    private void FindNearestEnemy()
    {
        float nearestDistance = Mathf.Infinity;
        Enemy nearestEnemy;
        foreach (var enemy in _enemies)
        {
            var distance = (enemy.transform.position - transform.position).sqrMagnitude;
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }
        transform.LookAt(nearestEnemy.transform.position);
    }
    
}
