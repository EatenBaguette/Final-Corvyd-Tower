using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }
    
    public override void EnterState()
    {
        Context.Animator.SetBool("isJumping", true);
        Debug.Log("Enter Jump State");
    }

    public override void UpdateState()
    {
        Context.HandleJump();
        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void ExitState()
    {
        Context.Animator.SetBool("isJumping", false);
    }

    public override void CheckSwitchStates()
    {
        if (!Context.CheckJump())
        {
            SetState(Factory.Idle);
        }
    }

    public override void InitSubState()
    {
        
    }
}