using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState CurrentEnemyState { get; private set; }
    public Enemy Enemy { get; private set; }
    public EnemyElementalBoss EnemyElementalBoss;

    public EnemyState LastState;
    //a constructor with Enemy as a parameter
    public EnemyStateMachine(Enemy enemy)
    {
        Enemy = enemy;
        EnemyElementalBoss = enemy as EnemyElementalBoss ?? null;
    }
    public void Initialize(EnemyState startEnemyState)
    {
        CurrentEnemyState = startEnemyState;
        CurrentEnemyState.Enter();
    }

    public bool ChangeState(EnemyState newEnemyState)
    {
        if (!CurrentEnemyState.CanExit)
        {
            //Debug.Log("Cannot exit current state" + CurrentEnemyState);
            return false;
        }
        LastState = CurrentEnemyState;
        CurrentEnemyState.Exit();
        CurrentEnemyState = newEnemyState;
        CurrentEnemyState.Enter();
        return true;
    }
}