using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    public enum FilterType
    {
        Simple,
        Rigid,
    }

    [System.Serializable]
    public class SimpleNoiseSettings
    {
        public float strenght = 1f;
        [Range(1, 8)]
        public int numberOfLayers = 1;
        public float baseRoughness = 1f;
        public float roughness = 2f;
        public float persistance = 0.5f;
        public Vector3 center;
        public float minValue;
    }

    [System.Serializable]
    public class RigidNoiseSettings : SimpleNoiseSettings
    {
        public float weightMultiplier;
    }

    public FilterType filterType;

    [ConditionalHide("filterType", (int)FilterType.Simple)]
    public SimpleNoiseSettings simpleNoiseSettings;
    [ConditionalHide("filterType", (int)FilterType.Rigid)]
    public RigidNoiseSettings rigidNoiseSettings;

}
