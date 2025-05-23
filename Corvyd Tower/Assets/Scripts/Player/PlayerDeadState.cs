using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    public PlayerDeadState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }
    
    public override void EnterState()
    {
        Context._gameManager.gameOver = true;
    }

    public override void UpdateState()
    {
      
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        
    }

    public override void InitSubState()
    {
        
    }
}