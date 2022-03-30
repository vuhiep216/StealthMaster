using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : EnemyController
{
    RangeAttackState rangeAttackState = new RangeAttackState();
    protected override void Init()
    {
        base.Init();
        rangeAttackState.Init(this);

    }
}
