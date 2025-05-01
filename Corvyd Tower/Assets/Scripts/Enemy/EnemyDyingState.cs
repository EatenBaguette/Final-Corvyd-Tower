// STATE MACHINE - CONCRETE STATE

using UnityEngine;

public class EnemyDyingState : EnemyBaseState
{
    public override void EnterState(EnemyStateMachine enemy)
    {
        enemy.IsAlive = false;
        enemy.Death();
        Debug.Log("Enter Enemy Dying State");
    }

    public override void UpdateState(EnemyStateMachine enemy) 
    {
        
    }
    
    public override void ExitState(EnemyStateMachine enemy)
    {
        Debug.Log("Exit Enemy Dying State");
    }
}
