using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public bool camCanMove = true;
    public bool playerCanShoot = true;

    [SerializeField] private GameObject spiderPrefab;
    [SerializeField] private Transform spawnArea;
    
    private List<EnemyStateMachine> _spiders = new List<EnemyStateMachine>();

    public bool isDay = true;
    public bool atKeep = true;
    private int _wildernessCombatSize = 0;
    [SerializeField] private TextMeshProUGUI _wildernessText;
    
    public bool gameOver = false;

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

    public void SetDay(bool fade)
    {
        if (!isDay)
        {
            if (fade)
            {
                lightingManager.StartCoroutine("FadeToDay");
            }
        }
        else
        {
            lightingManager.UpdateLighting(0.5f);
        }
        isDay = true;
        AkSoundEngine.SetState("TimeOfDay", "Day");
        DestroyAllSpiders();
    }

    public void SetNight()
    {
        isDay = false;
        AkSoundEngine.SetState("TimeOfDay", "Night");
        lightingManager.StartCoroutine("FadeToNight");
    }

    public void OnClickResume()
    {
        isPaused = !isPaused;
    }

    public void OnClickRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void CreateEnemy()
    {
        if (isDay)
        {
            if (!atKeep)
            {
                _wildernessCombatSize += 1;
                Math.Clamp(_wildernessCombatSize, 0, 2);
                AkSoundEngine.SetState("WildernessCombatLevel", "Level" + _wildernessCombatSize);
                _wildernessText.text = "Wilderness Combat Level: " + _wildernessCombatSize;
            }
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
            _spiders.Add(spiderInstance.GetComponent<EnemyStateMachine>());
            return spiderInstance;
        }
        return null;
    }

    public void DestroySpider()
    {
        if (isDay && !atKeep)
        {
            _wildernessCombatSize -= 1;
            Math.Clamp(_wildernessCombatSize, 0, 2);
            AkSoundEngine.SetState("WildernessCombatLevel", "Level" + _wildernessCombatSize);
            _wildernessText.text = "Wilderness Combat Level: " + _wildernessCombatSize;
        }
        if (_spiders != null && _spiders.Count > 0)
        {
            if (_spiders[0] != null)
            {
                _spiders[0].Death();
                _spiders.Remove(_spiders[0]);
            }
        }
    }

    public void DestroyAllSpiders()
    {
        foreach (EnemyStateMachine spider in _spiders)
        {
            spider.Death();
        }
    }
    

    public void SetCombatRTPC()
    {
        int spidersInRange = 0;
        foreach (EnemyStateMachine spider in _spiders)
        {
            if (spider.inRange)
            {
                spidersInRange++;
            }
        }
        if (spidersInRange > 0 && spidersInRange <= 5)
        {
            AkSoundEngine.SetState("KeepCombatState", "Combat");
            StartCoroutine(FadeCombatSize(spidersInRange));
        }
        else if (spidersInRange >= 5)
        {
            AkSoundEngine.SetRTPCValue("CombatSize", 100);
        }
        else
        {
            AkSoundEngine.SetState("KeepCombatState", "Explore");
            AkSoundEngine.SetRTPCValue("CombatSize", 0);
        }
    }

    public IEnumerator FadeCombatSize(int spidersInRange)
    {
        float fadeTime = 1.0f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            AkSoundEngine.SetRTPCValue("CombatSize", spidersInRange * 20 * (elapsedTime / fadeTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
