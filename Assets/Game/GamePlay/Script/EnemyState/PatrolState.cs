using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PatrolState : EnemyState
{
    protected Tweener tweener;
    protected Tween tween;

    public override void Enter()
    {

    }
    public override void Update()
    {
        
    }
    public override void Exit()
    {
        tweener.Pause();
        tween.Pause();
        Debug.Log(">>>>>");
    }
}
