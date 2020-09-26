using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ShapeSettings : ScriptableObject
{
    [System.Serializable]
    public struct NoiseLayer
    {
        public bool enabled;
        public bool useFirstLayerAsMask;
        public NoiseSettings noiseSettings;
    }

    public float radius = 1f;
    public NoiseLayer[] noiseLayers = new NoiseLayer[0];
}
