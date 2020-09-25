using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseFilter
{
    public NoiseSettings settings;

    private Noise noise = new Noise();

    public NoiseFilter(NoiseSettings noiseSettings)
    {
        settings = noiseSettings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0f;
        float frequency = settings.baseRoughness;
        float amplitude = 1f;

        for (int i = 0; i < settings.numberOfLayers; i++)
        {
            float v = noise.Evaluate(point * frequency + settings.center);
            noiseValue += (v + 1f) * 0.5f * amplitude;
            frequency *= settings.roughness;
            amplitude *= settings.persistance;
        }

        noiseValue = Mathf.Max(0f, noiseValue - settings.minValue);
        return noiseValue * settings.strenght;
    }
}
