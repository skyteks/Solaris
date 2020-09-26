using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ColorSettings : ScriptableObject
{
    [System.Serializable]
    public struct BiomeColorSettings
    {
        [System.Serializable]
        public class Biome
        {
            public Gradient gradient;
            public Color tint;
            [Range(0f, 1f)]
            public float startHeight;
            [Range(0f, 1f)]
            public float TintPercentage;
        }

        public Biome[] biomes;
        public NoiseSettings noise;
        public float noiseOffset;
        public float noiseStrenght;
        [Range(0f, 1f)]
        public float blendAmount;
    }

    public Material material;
    public BiomeColorSettings biomeColorSettings;
    public Gradient oceonColor;
}
