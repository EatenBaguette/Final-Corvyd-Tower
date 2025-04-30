using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GMTitleState : GMBaseState
{
    public GMTitleState(GMStateMachine currentContext, GMStateFactory factory)
        : base(currentContext, factory) { }

    public override void EnterState()
    {
        Debug.Log("Enter Title State");
    }

    public override void UpdateState()
    {
    }

    public override void FixedUpdateState()
    {
    }

    public override void ExitState()
    {
        SceneManager.UnloadSceneAsync(0);
        
        SceneManager.LoadScene(1);
        
        Time.timeScale = 1;
    }

    public override void OnGUIState()
    {
        
    }
    
    public override void CheckSwitchStates()
    {
        SetState(Factory.PlayState);
    }
}