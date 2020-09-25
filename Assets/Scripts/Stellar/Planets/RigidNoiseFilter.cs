using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidNoiseFilter : INoiseFilter
{
    public NoiseSettings.RigidNoiseSettings settings;

    private Noise noise = new Noise();

    public RigidNoiseFilter(NoiseSettings.RigidNoiseSettings noiseSettings)
    {
        settings = noiseSettings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0f;
        float frequency = settings.baseRoughness;
        float amplitude = 1f;
        float weight = 1f;

        for (int i = 0; i < settings.numberOfLayers; i++)
        {
            float v = 1f - Mathf.Abs(noise.Evaluate(point * frequency + settings.center));
            v = v * v;
            v *= weight;
            weight = Mathf.Clamp01(v * settings.weightMultiplier);
            noiseValue += v * amplitude;
            frequency *= settings.roughness;
            amplitude *= settings.persistance;
        }

        noiseValue = Mathf.Max(0f, noiseValue - settings.minValue);
        return noiseValue * settings.strenght;
    }
}
