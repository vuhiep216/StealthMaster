using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public enum EnemyType
    {
        Moving,
        Idle
    }
    [SerializeField] private EnemyType enemyType;
    EnemyState _currentState;
    PatrolState patrolState;
    AttackState attackState;
    DamagedState damagedState = new DamagedState();
    ChaseState chaseState = new ChaseState();
    [SerializeField]private List<Vector3> patrolPositions;
    [SerializeField]private bool isPatrolLoop;
    [SerializeField]NavMeshAgent _navMeshAgent;
    public NavMeshAgent navMeshAgent => _navMeshAgent;
    public CharactorAnimController characterController;
    public bool isDead;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        Init();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(_currentState!=null)
            _currentState.Update();
    }
    protected virtual void ChangeState(EnemyState newState)
    {
        if (newState == null)
            return;
        if (newState == _currentState)
            return;
        if (_currentState != null)
            _currentState.Exit();
        if (newState!=null)
            _currentState = newState;
        _currentState.Enter();
    }
    protected virtual void Init()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        characterController = GetComponentInChildren<CharactorAnimController>();
        patrolPositions.Add(transform.position);
        if (enemyType == EnemyType.Moving)
        {
            
            var patrol = GetComponentsInChildren<PatrolPosition>();
            foreach (var p in patrol)
            {
                patrolPositions.Add(p.transform.position);
            }
            PatrolMovingState patrolMovingState = new PatrolMovingState();
            patrolState = patrolMovingState;
            patrolState.Init(this);
            patrolMovingState.PatrolInit(patrolPositions, isPatrolLoop);
        }
        else
        {
            var rotate = GetComponentsInChildren<PatrolRotation>();
            foreach (var r in rotate)
            {
                patrolPositions.Add(r.transform.position);
            }
            PatrolIdleState patrolIdleState = new PatrolIdleState();
            patrolState = patrolIdleState;
            patrolState.Init(this);
            patrolIdleState.InitRotatePos(patrolPositions);
        }
        
        damagedState.Init(this);
        chaseState.Init(this);
        ChangeState(patrolState);
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            Debug.Log("???");
            ChangeState(damagedState);
        }
    }
    public void DeadAction()
    {
        transform.DOKill();
        navMeshAgent.enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        isDead = true;
        characterController.ChangeAnimDie();
    }
}
