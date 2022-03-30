using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedState : EnemyState
{
    public override void Enter()
    {
        _enemyController.DeadAction();
    }
    public override void Update()
    {

    }
    public override void Exit()
    {

    }

}
