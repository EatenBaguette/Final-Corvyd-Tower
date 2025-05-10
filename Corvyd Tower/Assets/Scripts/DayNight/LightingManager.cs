using UnityEngine;

[ExecuteInEditMode]
public class LightingManager : MonoBehaviour
{
    [SerializeField] private GMStateMachine _gameManager;
    [SerializeField] private Light _directionalLight;
    [SerializeField] private LightingPreset _preset;
    [SerializeField, Range(0, 24)] private float _timeOfDay;

    public float TimeOfDay
    {
        get => _timeOfDay;
        set => _timeOfDay = value;
    }

    private float _timer;
    [SerializeField] private float _nightTime = 200f;
    [SerializeField] private float _dayTime = 100f;
    [SerializeField] private float _dayStartHour = 6.0f;
    [SerializeField] private float _dayEndHour = 8.0f;

    private bool isDay = false;

    private void Start()
    {
        _timer = 0.0f;
        _gameManager= FindObjectOfType<GMStateMachine>();
        _timeOfDay = _dayStartHour;
        UpdateLighting(_timeOfDay/24f);
    }

    private void Update()
    {
        if (_preset == null)
        {
            return;
        }

        if (Application.isPlaying)
        {
            _timer += Time.deltaTime;
            if (_timer >= _dayTime + _nightTime)
            {
                _timer -= _dayTime + _nightTime;
            }
            
            if (_timeOfDay >= _dayStartHour && _timeOfDay <= _dayEndHour)
            {
                if (!isDay)
                {
                    isDay = true;
                    _gameManager.SetDay();
                }
                _timeOfDay += Time.deltaTime * (_dayEndHour - _dayStartHour) / _dayTime;
                
                _timeOfDay %= 24;
                UpdateLighting(_timeOfDay / 24f);
            }
            else
            {
                if (isDay)
                {
                    isDay = false;
                    _gameManager.SetNight();
                }

                _timeOfDay += Time.deltaTime * (24f - (_dayEndHour - _dayStartHour)) / _nightTime;
                _timeOfDay %= 24;
                UpdateLighting(_timeOfDay / 24f);
            }
        }
        else
        {
            UpdateLighting(_timeOfDay / 24f);
        }
    }

    public void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = _preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = _preset.FogColor.Evaluate(timePercent);

        if (_directionalLight != null)
        {
            _directionalLight.color = _preset.DirectionalColor.Evaluate(timePercent);
            _directionalLight.transform.localRotation =
                Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }
    }
    
    private void OnValidate()
    {
        if (_directionalLight != null)
        {
            return;
        }

        if (RenderSettings.sun != null)
        {
            _directionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    _directionalLight = light;
                    return;
                }
            }
        }
    }
}
