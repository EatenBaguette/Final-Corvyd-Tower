using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEditor.Presets;
using UnityEngine;

[ExecuteInEditMode]
public class LightingManager : MonoBehaviour
{
    [SerializeField] private Light _directionalLight;
    [SerializeField] private LightingPreset _preset;
    [SerializeField, Range(0, 24)] private float _timeOfDay;

    private void Start()
    {
        _timeOfDay = 12f;
    }
    private void Update()
    {
        if (_preset == null)
        {
            return;
        }

        //if (Application.isPlaying)
       // {
            //_timeOfDay %= 24;
            //UpdateLighting(_timeOfDay/24);

            //_timeOfDay += Time.deltaTime;
            //_timeOfDay %= 24;
            //UpdateLighting(_timeOfDay/24);
        //}
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
    public IEnumerator FadeToDay()
    {
        float transitionTime = 10.0f;
        float elapsedTime = 0.0f;
        
        while (elapsedTime < transitionTime)
        {
            UpdateLighting(0.5f * (elapsedTime / transitionTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        UpdateLighting(0.5f);
    }

    public IEnumerator FadeToNight()
    {
        float transitionTime = 0.5f;
        float elapsedTime = 0.0f;

        while (elapsedTime < transitionTime)
        {
            UpdateLighting(0.5f - (0.5f * (elapsedTime / transitionTime)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        UpdateLighting(0f);
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
