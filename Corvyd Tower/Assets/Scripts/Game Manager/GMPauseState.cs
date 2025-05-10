using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMPauseState : GMBaseState
{
    public GMPauseState(GMStateMachine currentContext, GMStateFactory factory)
        : base(currentContext, factory) { }

    public override void EnterState()
    {
        Debug.Log("EnterPauseState");
        
        Time.timeScale = 0;
        CurrentContext.pauseScreen.SetActive(true);
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        CurrentContext.camCanMove = false;
        CurrentContext.playerCanShoot = false;
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
        Time.timeScale = 1;
    }

    public override void OnGUIState()
    {
        
    }


    public override void CheckSwitchStates()
    {
        CurrentContext.CheckPause();
        if (!CurrentContext.isPaused)
        {
            SetState(CurrentContext.PreviousState);
        }
    }
}