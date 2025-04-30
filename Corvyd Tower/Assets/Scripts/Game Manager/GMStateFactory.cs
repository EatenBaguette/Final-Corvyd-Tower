using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class GMStateFactory
{
    private GMStateMachine _context;
    
    // This constructor gives the state factory context from the game object the state machine is attached to
    public GMStateFactory(GMStateMachine context)
    {
        _context = context;
    }
    
    
    // These variables hold the possible states, creating them in memory when needed
    private GMGameOverState _gameOverState;
    public GMGameOverState  GameOverState
    {
        get
        {
            _gameOverState ??= new GMGameOverState(_context, this);
            return _gameOverState;
        }
    }
    private GMPauseState _pauseState;
    public GMPauseState  PauseState
    {
        get
        {
            _pauseState ??= new GMPauseState(_context, this);
            return _pauseState;
        }
    }
    
    private GMPlayState _playState;
    public GMPlayState  PlayState
    {
        get
        {
            _playState ??= new GMPlayState(_context, this);
            return _playState;
        }
    }
    
    private GMTitleState _titleState;
    public GMTitleState  TitleState
    {
        get
        {
            _titleState ??= new GMTitleState(_context, this);
            return _titleState;
        }
    }
}