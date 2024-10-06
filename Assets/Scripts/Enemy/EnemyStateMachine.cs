using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState CurrentEnemyState { get; private set; }
    public Enemy Enemy { get; private set; }
    //a constructor with Enemy as a parameter
    public EnemyStateMachine(Enemy enemy)
    {
        Enemy = enemy;
    }
    public void Initialize(EnemyState startEnemyState)
    {
        CurrentEnemyState = startEnemyState;
        CurrentEnemyState.Enter();
    }

    public void ChangeState(EnemyState newEnemyState)
    {
        CurrentEnemyState.Exit();
        CurrentEnemyState = newEnemyState;
        CurrentEnemyState.Enter();
    }
}