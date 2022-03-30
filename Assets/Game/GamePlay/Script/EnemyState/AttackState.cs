using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackState : EnemyState
{
    public override void Enter()
    {
        
    }
    public override void Update()
    {
        if (!CheckCanAttack())
            return;
        OnAnimatedAttackAnim();
    }
    public override void Exit()
    {
        OnAnimatedAttackEnd();
    }
    public bool CheckCanAttack ()
    {
        return false;
    }
    public void OnAnimatedAttackEnd()
    {

    }
    public abstract void OnAnimatedAttackAnim();
}
