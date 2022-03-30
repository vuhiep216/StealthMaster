using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyController
{
    MeleeAttackState meleeAttackState = new MeleeAttackState();
    protected override void Init()
    {
        base.Init();
        meleeAttackState.Init(this);
    }
}