using System.ComponentModel.Design;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "LightingPreset", menuName = "Scriptables/LightingPreset", order = 1)]
public class LightingPreset : ScriptableObject
{
    public Gradient AmbientColor;
    public Gradient DirectionalColor;
    public Gradient FogColor;
}
