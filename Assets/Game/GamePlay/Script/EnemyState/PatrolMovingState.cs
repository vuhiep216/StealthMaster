using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PatrolMovingState : PatrolState
{
    public enum MiniPatrolState
    {
        Move,
        Turn
    }
    private MiniPatrolState _currentState;
    float speedRotate = 180f;
    List<Vector3> _listPatrolPos;
    bool _isLoop;
    bool _isReverse;
    int indexNextPos=0;
    private void EnterMiniState()
    {
        switch (_currentState)
        {
            case MiniPatrolState.Move:
                if (_isReverse)
                {
                    if (indexNextPos >= _listPatrolPos.Count-1)
                    {
                        indexNextPos = _listPatrolPos.Count - 2;
                    }
                    else
                    {
                        if (indexNextPos > 0)
                            indexNextPos--;
                        else
                            _isReverse = false;
                    }

                }
                else
                {
                    if (!_isLoop)
                    {
                        if (indexNextPos >= _listPatrolPos.Count-1)
                        {
                            _isReverse = true;
                        }
                        else
                        {
                            indexNextPos++;
                        }
                    }
                    else
                    {
                        if (indexNextPos >= _listPatrolPos.Count-1)
                            indexNextPos = 0;
                        else
                        {
                            indexNextPos++;
                        }
                    }
                }
                _enemyController.characterController.ChangeAnimWalk();
                break;
            case MiniPatrolState.Turn:
                _enemyController.characterController.ChangeAnimIdle();
                var v = (_listPatrolPos[indexNextPos] - _enemyController.transform.position).normalized;
                float angle = Vector3.Angle(v, _enemyController.transform.forward);
                tweener=_enemyController.transform.DOLookAt
                    (new Vector3(_listPatrolPos[indexNextPos].x, _enemyController.transform.position.y,_listPatrolPos[indexNextPos].z)
                    , angle / speedRotate).OnComplete(() =>
                    {
                       tween= DOVirtual.DelayedCall(1f,()=> ChangeMiniState(MiniPatrolState.Move));
                    });
                break;
        }
    }
    private void ExitMiniState()
    {
        switch (_currentState)
        {
            case MiniPatrolState.Move:
                break;
            case MiniPatrolState.Turn:
                break;
        }
    }
    private void ChangeMiniState (MiniPatrolState newState)
    {
        if (newState == _currentState)
            return;
        ExitMiniState();
        _currentState = newState;
        EnterMiniState();

    }
    private void UpdateMiniState()
    {
        switch (_currentState)
        {
            case MiniPatrolState.Move:
                _enemyController.navMeshAgent.SetDestination(_listPatrolPos[indexNextPos]);
                var d = Vector3.Distance(_enemyController.transform.position, _listPatrolPos[indexNextPos]);
                if (d <= 0.1f)
                {
                    ChangeMiniState(MiniPatrolState.Turn);
                }
                break;
            case MiniPatrolState.Turn:
                break;
        }
    }
    public void PatrolInit(List<Vector3> listPos,bool isLoop)
    {
        _listPatrolPos = listPos;
        _isLoop = isLoop;
    }
    public override void Enter()
    {
        _enemyController.navMeshAgent.SetDestination(_listPatrolPos[0]);
    }
    public override void Update()
    {
        UpdateMiniState();
    }
    public override void Exit()
    {
        base.Exit();
    }
}
