// STATE MACHINE - CONCRETE STATE

using UnityEngine;

public class EnemyDyingState : EnemyBaseState
{
    public override void EnterState(EnemyStateMachine enemy)
    {
        Debug.Log("Enter Enemy Dying State");
        enemy.IsAlive = false;
        enemy.animator.SetBool("isDying", true);
        enemy.StopAllCoroutines();
        enemy._target = null;
        enemy.inRange = false;
        enemy.StartCoroutine("Dying");
    }

    public override void UpdateState(EnemyStateMachine enemy) 
    {
        
    }
    
    public override void ExitState(EnemyStateMachine enemy)
    {
        Debug.Log("Exit Enemy Dying State");
    }
}
