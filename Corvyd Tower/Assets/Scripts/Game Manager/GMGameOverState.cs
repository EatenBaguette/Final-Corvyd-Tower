using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMGameOverState : GMBaseState
{
    public GMGameOverState(GMStateMachine currentContext, GMStateFactory factory)
        : base(currentContext, factory) { }

    public override void EnterState()
    {
    }

    public override void UpdateState()
    {
        // call necessary GMStateMachine Methods here
        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {
    }

    public override void ExitState()
    {
    }

    public override void OnGUIState()
    {
        
    }


    public override void CheckSwitchStates()
    {
    }
}