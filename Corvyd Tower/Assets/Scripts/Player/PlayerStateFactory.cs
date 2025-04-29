using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateFactory
{
  private PlayerStateMachine _context;
  public PlayerStateFactory(PlayerStateMachine currentContext)
  {
    _context = currentContext;
  }

  private PlayerBaseState _idle;
  public PlayerBaseState Idle
  {
    get
    {
      _idle ??= new PlayerIdleState(_context, this);
      return _idle;
    }
  }
  
  private PlayerBaseState _walk;
  public PlayerBaseState Walk
  {
    get
    {
      _walk ??= new PlayerWalkState(_context, this);
      return _walk;
    }
  }
  
  private PlayerBaseState _run;
  public PlayerBaseState Run
  {
    get
    {
      _run ??= new PlayerRunState(_context, this);
      return _run;
    }
  }
  
  private PlayerBaseState _jump;
  public PlayerBaseState Jump
  {
    get
    {
      _jump ??= new PlayerJumpState(_context, this);
      return _jump;
    }
  }
  
  private PlayerBaseState _alive;
  public PlayerBaseState Alive
  {
    get
    {
      _alive ??= new PlayerAliveState(_context, this);
      return _alive;
    }
  }
  
  private PlayerBaseState _dead;
  public PlayerBaseState Dead
  {
    get
    {
      _dead ??= new PlayerDeadState(_context, this);
      return _dead;
    }
  }
}
