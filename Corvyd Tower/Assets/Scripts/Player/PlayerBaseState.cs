using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState
{
    private bool _isRootState = false;

    protected bool IsRootState
    {
        set { _isRootState = value; }
    }

    private PlayerStateMachine _context;

    protected PlayerStateMachine Context
    {
        get => _context;
    }

    private PlayerStateFactory _factory;

    protected PlayerStateFactory Factory
    {
        get => _factory;
    }

    protected PlayerBaseState _currentSubState;
    protected PlayerBaseState _currentSuperState;
    
    public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    {
        _context = currentContext;
        _factory = playerStateFactory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();

    public abstract void FixedUpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
    public abstract void InitSubState();

    public void UpdateStates()
    {
        if (_currentSubState != null)
        {
            _currentSubState.UpdateState();
        }
    }

    protected void SetState(PlayerBaseState newState)
    {
        ExitState();

        newState.EnterState();

        if (_isRootState)
        {
            _context.CurrentState = newState;
        }
        else if (_currentSuperState != null)
        {
            _currentSuperState.SetSubState(newState);
        }
    }

    protected void SetSuperState(PlayerBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(PlayerBaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }

}