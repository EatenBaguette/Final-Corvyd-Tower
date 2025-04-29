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
    }

    public override void UpdateState()
    {
        UpdateStates();
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void ExitState()
    {
        
    }

    public override void CheckSwitchStates()
    {
        if (Context.Health <= 0)
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
