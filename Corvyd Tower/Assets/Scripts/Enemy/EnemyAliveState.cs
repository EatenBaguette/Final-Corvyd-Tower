// STATE MACHINE - CONCRETE STATE
using UnityEngine;

public class EnemyAliveState : EnemyBaseState
{
    public override void EnterState(EnemyStateMachine enemy)
    {
        enemy.IsAlive = true;

        Debug.Log("Enter Enemy Alive State");

        enemy.SetNavigation();
    }

    public override void UpdateState(EnemyStateMachine enemy)
    {
        enemy.animator.SetFloat("Speed", enemy._agent.velocity.sqrMagnitude);
        enemy.CheckRange();
    }

    public override void ExitState(EnemyStateMachine enemy)
    {
        Debug.Log("Exit Enemy ALive State");
        enemy.IsAlive = false;
    }
}
