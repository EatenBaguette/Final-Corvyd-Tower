using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GMPlayState : GMBaseState
{
    public GMPlayState(GMStateMachine currentContext, GMStateFactory factory)
        : base(currentContext, factory) { }

    public override void EnterState()
    {
        Debug.Log("EnterPlayState");
        
        Time.timeScale = 1;
        
        CurrentContext.gameOverScreen.SetActive(false);
        CurrentContext.pauseScreen.SetActive(false);
        CurrentContext.SetDay();
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
        CurrentContext.CheckPause();
        if (CurrentContext.isPaused)
        {
            SetState(Factory.PauseState);
        }
    }
}