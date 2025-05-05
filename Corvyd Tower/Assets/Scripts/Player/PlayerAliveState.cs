using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAliveState : PlayerBaseState
{
    public PlayerAliveState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }
    
    public override void EnterState()
    {
        IsRootState = true;

        InitSubState();
        
        Debug.Log("PlayerAliveState EnterState");

        Context._gameOver = false;
        Context._gameManager.gameOver = false;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        UpdateStates();
        Context.HandleShoot();
        Context.HandleSmash();
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void ExitState()
    {
        
    }

    public override void CheckSwitchStates()
    {
        if (Context.Health <= 0 || Context._gameOver)
        {
            SetState(Factory.Dead);
        }
    }

    public override void InitSubState()
    {
        SetSubState(Factory.Idle);
        _currentSubState.EnterState();
    }
}
