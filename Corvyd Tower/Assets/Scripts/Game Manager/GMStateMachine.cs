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
    private int _spidersToSpawn;
    private List<EnemyStateMachine> _toRemove = new List<EnemyStateMachine>();
    
    [SerializeField] private GameObject powerupPrefab;
    [SerializeField] private Transform powerupSpawnArea;
    private List<Powerup> _powerups = new List<Powerup>();
    private int _powerupsToSpawn;
    
    public bool isDay = false;
    public bool atKeep = true;
    private int _wildernessCombatSize = 0;
    [SerializeField] private int _dayNumber = 0;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _powerupText;
    
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
        Debug.Log("Set Day");
        if (isDay) { return;}
        else
        {
            isDay = true;
            _dayNumber++;
            _scoreText.text = "Day " + _dayNumber;
            UpdateSpidersToSpawn();
            UpdatePowerupsToSpawn();
            SpawnPowerups(_powerupsToSpawn);
            CheckInactivePowerups();
            StartCoroutine(DelayedSetDay());
            KillAllSpiders();
        }
    }

    private IEnumerator DelayedSetDay()
    {
        yield return new WaitForSeconds(4f);
        AkSoundEngine.SetState("TimeOfDay", "Day");
        CheckInactiveSpiders();
    }

    public void SetNight()
    {
        Debug.Log("Set Night");
        if (!isDay) { return;}
        else
        {
            isDay = false;
            AkSoundEngine.SetState("TimeOfDay", "Night");
            //DestroyAllPowerups();
            createEnemy(_spidersToSpawn);
        }
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
        //if (isDay)
       // {
           // if (!atKeep)
          //  {
          //      _wildernessCombatSize += 1;
          //      Math.Clamp(_wildernessCombatSize, 0, 2);
           //     AkSoundEngine.SetState("WildernessCombatLevel", "Level" + _wildernessCombatSize);
            //    _scoreText.text = "Wilderness Combat Level: " + _wildernessCombatSize;
           // }
       // }
      //  else
      //  {
        //    createEnemy(_spidersToSpawn); 
       // }
       createEnemy(1);
    }
    private void createEnemy(int amount)
    {
        for (int i = 0; i < amount; i++)
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
            }
        }
    }

    //public void DestroySpider()
    //{
        //if (isDay && !atKeep)
        //{
           // _wildernessCombatSize -= 1;
          //  Math.Clamp(_wildernessCombatSize, 0, 2);
           // AkSoundEngine.SetState("WildernessCombatLevel", "Level" + _wildernessCombatSize);
         //   _scoreText.text = "Wilderness Combat Level: " + _wildernessCombatSize;
      //  }
       // if (_spiders != null && _spiders.Count > 0)
      //  {
        //    if (_spiders[0] != null)
        //    {
         //       _spiders[0].Death();
         //       _spiders.Remove(_spiders[0]);
         //   }
       // }
  //  }
    

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

    private void UpdateSpidersToSpawn()
    {
        float x = _dayNumber;
        _spidersToSpawn = Mathf.CeilToInt(
            (1.77f)
            + (0.787f * x)
            + (0.824f * Mathf.Pow(x, 2))
            + (-0.0251f * Mathf.Pow(x, 3))
            + (0.000333f * Mathf.Pow(x, 4))
        );
    }

    private void UpdatePowerupsToSpawn()
    {
        float x = _dayNumber;
        _powerupsToSpawn = Mathf.CeilToInt(
            (1.77f)
            + (0.787f * (x-10f))
            + (0.824f * Mathf.Pow((x-10f), 2))
            + (-0.0251f * Mathf.Pow((x-10f), 3))
            + (0.000333f * Mathf.Pow((x-10f), 4))
        );
    }

    private void SpawnPowerups(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 pos = powerupSpawnArea.position;
            Vector3 scale = powerupSpawnArea.localScale;
            pos.x = Random.Range((pos.x - scale.x/2),(pos.x + scale.x/2));
            pos.z = Random.Range((pos.z - scale.z/2),(pos.z + scale.z/2));
            RaycastHit hit;
            if (Physics.Raycast(pos, Vector3.down, out hit))
            {
                Vector3 spawnPos = hit.point;
                GameObject powerupInstance = Instantiate(powerupPrefab, spawnPos, Quaternion.identity);
                _powerups.Add(powerupInstance.GetComponent<Powerup>());
            }
        }
    }

    private void CheckInactiveSpiders()
    {
        int count = _spiders.Count;
        for (int i = count - 1; i >= 0; i--)
        {
            EnemyStateMachine enemy = _spiders[i];
            if (enemy.gameObject.activeInHierarchy == false)
            {
                _spiders.RemoveAt(i);
                Destroy(enemy.gameObject);
            }
        }
    }

    private void KillAllSpiders()
    {
        foreach (EnemyStateMachine spider in _spiders)
        {
            if (spider.gameObject.activeInHierarchy == true)
            {
                spider.Death();
            }
        }
    }
    private void CheckInactivePowerups()
    {
        int count = _powerups.Count;
        for (int i = count - 1; i >= 0; i--)
        {
            Powerup powerup = _powerups[i];
            if (powerup.gameObject.activeInHierarchy == false)
            {
                _powerups.RemoveAt(i);
                Destroy(powerup.gameObject);
            }
        }
    }
}
