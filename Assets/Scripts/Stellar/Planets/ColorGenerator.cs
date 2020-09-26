using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGenerator
{
    private ColorSettings settings;
    private Texture2D texture;
    private const int textureResolution = 50;
    private INoiseFilter biomeNoiseFilter;

    public void UpdateSettings(ColorSettings colorSettings)
    {
        settings = colorSettings;
        if (texture == null || texture.height != settings.biomeColorSettings.biomes.Length)
        {
            texture = new Texture2D(textureResolution * 2, settings.biomeColorSettings.biomes.Length, TextureFormat.RGBA32, false);
        }
        biomeNoiseFilter = NoiseFilterFactory.CreateNoiseFilter(settings.biomeColorSettings.noise);
    }

    public void UpdateElevation(MinMax elevationMinMax)
    {
        settings.material.SetVector("_elevationMinMax", new Vector4(elevationMinMax.min, elevationMinMax.max));
    }

    public void UpdateColors()
    {
        Color[] colors = new Color[texture.width * texture.height];
        int colorIndex = 0;
        foreach (ColorSettings.BiomeColorSettings.Biome biome in settings.biomeColorSettings.biomes)
        {
            for (int i = 0; i < textureResolution * 2; i++)
            {
                Color gradientColor;
                if (i < textureResolution)
                {
                    gradientColor = settings.oceonColor.Evaluate(i / (textureResolution - 1f));
                }
                else
                {
                    gradientColor = biome.gradient.Evaluate((i - textureResolution) / (textureResolution - 1f));
                }
                Color tintColor = biome.tint;
                colors[colorIndex] = gradientColor * (1f - biome.TintPercentage) + tintColor * biome.TintPercentage;
                colorIndex++;
            }
        }
        texture.SetPixels(colors);
        texture.Apply();
        settings.material.SetTexture("_texture", texture);
    }

    public float BiomePercentageFromPoint(Vector3 pointOnUnitSphere)
    {
        float heightPercentage = (pointOnUnitSphere.y + 1f) / 2f;
        heightPercentage += (biomeNoiseFilter.Evaluate(pointOnUnitSphere) - settings.biomeColorSettings.noiseOffset) * settings.biomeColorSettings.noiseStrenght;
        float biomeIndex = 0;
        int biomesCount = settings.biomeColorSettings.biomes.Length;
        float blendRange = settings.biomeColorSettings.blendAmount / 2f + 0.001f;

        for (int i = 0; i < biomesCount; i++)
        {
            float distance = heightPercentage - settings.biomeColorSettings.biomes[i].startHeight;
            float weight = Mathf.InverseLerp(-blendRange, blendRange, distance);
            biomeIndex *= (1f - weight);
            biomeIndex += i * weight;
        }

        return biomeIndex / Mathf.Max(1, biomesCount - 1);
    }
}
