using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyController _enemyController;
    public virtual void Init(EnemyController enemy)
    {
        _enemyController = enemy;
    }
    public virtual void Enter()
    {

    }
    public virtual void Update()
    {

    }
    public virtual void Exit()
    {

    }
}
