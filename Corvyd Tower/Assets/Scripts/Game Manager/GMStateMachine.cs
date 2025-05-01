using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

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

    private LightingManager lightingManager;

    [SerializeField] private GameObject spiderPrefab;
    [SerializeField] private Transform spawnArea;
    
    private List<GameObject> _spiders = new List<GameObject>();

    public bool isDay = true;

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
            lightingManager = GetComponent<LightingManager>();
            spawnArea = GameObject.Find("Spawn Area").transform;
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

    public void SetDay()
    {
        isDay = true;
        lightingManager.UpdateLighting(0.5f);
        DestroyAllSpiders();
    }

    public void SetNight()
    {
        isDay = false;
        lightingManager.UpdateLighting(0f);
    }

    public void OnClickResume()
    {
        isPaused = !isPaused;
    }

    public void CreateEnemy()
    {
        if (isDay)
        {
            return;
        }
        else
        {
            createEnemy(); 
        }
    }
    private GameObject createEnemy()
    {
        Vector3 pos = spawnArea.position;
        Vector3 scale = spawnArea.localScale;
        pos.x = Random.Range((pos.x - scale.x/2),(pos.x + scale.x/2));
        pos.z = Random.Range((pos.z - scale.z/2),(pos.z + scale.z/2));
        RaycastHit hit;
        if (Physics.Raycast(pos, Vector3.down, out hit))
        {
            Vector3 spawnPos = hit.point;
            GameObject spiderInstance = Instantiate(spiderPrefab, spawnPos, Quaternion.identity);
            
            _spiders.Add(spiderInstance);
            return spiderInstance;
        }
        return null;
    }

    public void DestroySpider()
    {
        if (_spiders[0] != null)
        {
            _spiders[0].GetComponent<EnemyStateMachine>().Death();
            _spiders.Remove(_spiders[0]);
        }
    }

    public void DestroyAllSpiders()
    {
        foreach (GameObject spider in _spiders)
        {
            spider.GetComponent<EnemyStateMachine>().Death();
        }
    }
}
