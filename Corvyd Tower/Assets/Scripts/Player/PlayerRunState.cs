using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }
    
    public override void EnterState()
    {
        Context.Animator.SetBool("isRunning", true);
        Debug.Log("Entering Run State");
        AkSoundEngine.PostEvent("Play_RunSteps", Context.gameObject);
    }

    public override void UpdateState()
    {
        Context.HandleRun();
        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void ExitState()
    {
        Context.Animator.SetBool("isRunning", false);
        AkSoundEngine.PostEvent("Stop_RunSteps", Context.gameObject);
    }

    public override void CheckSwitchStates()
    {
        if (!Context.CheckSprint())
        {
            SetState(Factory.Walk);
        }

        if (Context.CheckJump())
        {
            SetState(Factory.Jump);
        }

        if (!Context.CheckInput())
        {
            SetState(Factory.Idle);
        }
    }

    public override void InitSubState()
    {
        
    }
}