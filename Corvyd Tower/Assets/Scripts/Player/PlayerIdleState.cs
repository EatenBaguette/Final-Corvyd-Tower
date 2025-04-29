using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }
    
    public override void EnterState()
    {
        Context.Animator.SetBool("isIdle", true);
        Debug.Log("Enter Idle State");
    }

    public override void UpdateState()
    {
        Context.HandleIdle();
        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void ExitState()
    {
        Context.Animator.SetBool("isIdle", false);
    }

    public override void CheckSwitchStates()
    {
        if (Context.CheckInput())
        {
            SetState(Factory.Walk);
        }

        if (Context.CheckJump())
        { 
            SetState(Factory.Jump);
        }
    }

    public override void InitSubState()
    {
        
    }
}