public abstract class GMBaseState
{
    private GMStateMachine _currentContext;

    protected GMStateMachine CurrentContext { get => _currentContext; }

    private GMStateFactory _factory;
   
    protected GMStateFactory Factory { get => _factory; }


    public GMBaseState(GMStateMachine currentContext, GMStateFactory factory)
    {
        _currentContext = currentContext;
        _factory = factory;
    }
   
    // All the methods
    public abstract void EnterState();
   
    public abstract void UpdateState();
   
    public abstract void FixedUpdateState();
   
    public abstract void ExitState();
   
    public abstract void OnGUIState();
   
    public abstract void CheckSwitchStates();
   
    // Allow the derived state to change the state
    protected void SetState(GMBaseState newState)
    {
        ExitState();
        
        _currentContext.PreviousState = _currentContext.CurrentState;
        
        _currentContext.CurrentState = newState;
      
        newState.EnterState();
    }
}