using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }
    
    public override void EnterState()
    {
        Context.Animator.SetBool("isWalking", true);
        Debug.Log("PlayerWalkState EnterState");
    }

    public override void UpdateState()
    {
        Context.HandleWalk();
        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void ExitState()
    {
        Context.Animator.SetBool("isWalking", false);
    }

    public override void CheckSwitchStates()
    {
        if (!Context.CheckInput())
        {
            SetState(Factory.Idle);
        }

        if (Context.CheckJump())
        {
            SetState(Factory.Jump);
        }

        if (Context.CheckSprint())
        {
            SetState(Factory.Run);
        }
    }

    public override void InitSubState()
    {
        
    }
}