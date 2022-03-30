using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PatrolIdleState : PatrolState
{
    float speedRotate = 90;
    List<Vector3> _listRoatePos;
    int indexNextPos = 0;
    public void InitRotatePos(List<Vector3> listPos)
    {
        _listRoatePos = listPos;
    }
    public override void Enter()
    {
        RotateEnemy();
    }
    public override void Update()
    {

    }
    public override void Exit()
    {
        base.Exit();
    }
    private void RotateEnemy()
    {
        //if (_enemyController.isDead)
        //    return;
        indexNextPos++;
        if (indexNextPos >= _listRoatePos.Count)
        {
            indexNextPos = 0;
        }
        var v = (_listRoatePos[indexNextPos] - _enemyController.transform.position).normalized;
        float angle = Vector3.Angle(v, _enemyController.transform.forward);
        tweener= _enemyController.transform.DOLookAt
            (new Vector3(_listRoatePos[indexNextPos].x, _enemyController.transform.position.y, _listRoatePos[indexNextPos].z)
            , angle / speedRotate).OnComplete(() =>
            {
               tween =  DOVirtual.DelayedCall(1f, () => {
                    RotateEnemy();
                });
            });
    }
}
