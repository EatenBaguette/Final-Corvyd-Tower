using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GMStateMachine : MonoBehaviour
{
    public static GMStateMachine Instance;
    
    /******************* STATES *******************/
    
    private GMBaseState _currentState;
    private GMBaseState _previousState;
    
    private GMStateFactory _states;

    public GMBaseState CurrentState
    {
        get => _currentState;
        set => _currentState = value;
    }

    public GMBaseState PreviousState
    {
        get => _previousState;
        set => _previousState = value;
    }
    
    public GameObject pauseScreen;
    public GameObject gameOverScreen;

    void Awake()
    {
        if (Instance != null && Instance == this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        
        _states = new GMStateFactory(this);
    }

    public bool isPaused = false;
    
    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            _currentState = _states.TitleState;
            _currentState.EnterState();
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            _currentState = _states.PlayState;
            _currentState.EnterState();
        }
    }

    void Update()
    {
        _currentState.UpdateState();
    }

    void FixedUpdate()
    {
        _currentState.FixedUpdateState();
    }

    void OnGUI()
    {
        _currentState.OnGUIState();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OnClickPlay()
    {
        _currentState.CheckSwitchStates();
    }

    public void CheckPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
        }
    }

    public void OnClickResume()
    {
        isPaused = !isPaused;
    }
}
