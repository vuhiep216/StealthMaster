using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private DynamicJoystick _dynamicJoystick;
    [SerializeField]private int _hpBase=100;
    private float _speedWalk=0;
    private float _speedRun=4;
    private int _hpCurrent;
    NavMeshAgent _navMeshAgent;
    private PlayerState state;
    CharactorAnimController characterController;
    [SerializeField]private List<EnemyController> _enemyControllers;
    ExitController exitController;
    [SerializeField]private float _rangeAttack=0.5f;
    [SerializeField]private float _attackTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        characterController = GetComponentInChildren<CharactorAnimController>();
        _enemyControllers = FindObjectsOfType<EnemyController>().ToList();
        exitController = FindObjectOfType<ExitController>();
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == PlayerState.Die) return;
        if (state == PlayerState.Win) return;
        Vector3 dir = new Vector3(_dynamicJoystick.Direction.x,0,_dynamicJoystick.Direction.y);
        var moveDir = dir * _speed;
        _navMeshAgent.velocity = moveDir;
        if (_hpCurrent <= 0)
            ChangeState(PlayerState.Die);
        //if (state == PlayerState.Attack) return;
        UpdateState();
    }
    private void Init()
    {
        ChangeState(PlayerState.Init);
        _hpCurrent = _hpBase;
    }
    private EnemyController ClosetEnemy()
    {
        var _new = _enemyControllers.OrderBy(e=>Vector3.Distance(e.transform.position,transform.position)).ToList();
        var enemy = _new.FirstOrDefault(e => e.isDead == false);
        if (enemy != null)
            return enemy;
        else return null;
    }
    public void TryAttack()
    {
        if (ClosetEnemy() == null)
            return;
        if (Vector3.Distance(transform.position, ClosetEnemy().transform.position) < _rangeAttack)
        {
            ChangeState(PlayerState.Attack);
        }
    }
    public void UpdateState()
    {
        switch (state)
        {
            case PlayerState.Init:
                ChangeState(PlayerState.Idle);
                break;
            case PlayerState.Idle:
                if (_navMeshAgent.velocity.magnitude > _speedWalk)
                {
                    ChangeState(PlayerState.Walk);
                }
                TryAttack();
                break;
            case PlayerState.Walk:
                if (_navMeshAgent.velocity.magnitude <= _speedWalk)
                {
                    ChangeState(PlayerState.Idle);
                }
                if (_navMeshAgent.velocity.magnitude > _speedRun)
                {
                    ChangeState(PlayerState.Run);
                }
                TryAttack();
                break;
            case PlayerState.Run:
                if (_navMeshAgent.velocity.magnitude <= _speedRun)
                {
                    ChangeState(PlayerState.Walk);
                }
                TryAttack();
                break;
            case PlayerState.Attack:
                if (ClosetEnemy() == null)
                    return;
                if (ClosetEnemy().isDead)
                    ChangeState(PlayerState.Idle);
                transform.LookAt(new Vector3( ClosetEnemy().transform.position.x,0,ClosetEnemy().transform.position.z));
                _attackTime -= Time.deltaTime;
                if (_attackTime <= 0)
                {
                    ChangeState(PlayerState.Idle);
                }
                break;
            case PlayerState.Die:
                break;
            case PlayerState.Win:
                if (Vector3.Distance(gameObject.transform.position, exitController.transform.position) < 0.5f)
                {
                    ChangeState(PlayerState.Win);
                }
                TryAttack();
                break;
            default:
                break;
        }
    }
    public void ExitState()
    {
        switch (state)
        {
            case PlayerState.Init:
                break;
            case PlayerState.Idle:
                break;
            case PlayerState.Walk:
                break;
            case PlayerState.Run:
                break;
            case PlayerState.Attack:
                _attackTime = 1f;
                break;
            case PlayerState.Die:
                break;
            case PlayerState.Win:
                break;
            default:
                break;
        }
    }
    public void EnterState()
    {
        switch (state)
        {
            case PlayerState.Init:
                break;
            case PlayerState.Idle:
                characterController.ChangeAnimIdle();
                break;
            case PlayerState.Walk:
                characterController.ChangeAnimWalk();
                break;
            case PlayerState.Run:
                characterController.ChangeAnimRun();
                break;
            case PlayerState.Attack:
                characterController.ChangeAnimAttack();
                StartCoroutine(EndAttack());
                break;
            case PlayerState.Die:
                break;
            case PlayerState.Win:
                characterController.ChangeAnimVictory();
                break;
            default:
                break;
        }
    }
    void ChangeState(PlayerState newState)
    {
        if (newState == state) return;
        ExitState();
        state = newState;
        EnterState();
    }
    IEnumerator EndAttack()
    {
        yield return new WaitForSeconds(2);
        ChangeState(PlayerState.Idle);
    }
}
public enum PlayerState
{
    Init,
    Idle,
    Walk,
    Run,
    Attack,
    Die,
    Win
}