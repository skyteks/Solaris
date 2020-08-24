using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class SpeedEffect : MonoBehaviour
{
    private ParticleSystem effect;
    private ParticleSystem.MainModule mainModule;
    private ParticleSystem.EmissionModule emissionModule;
    public Rigidbody rigid;
    public AnimationCurve rateCurve;
    public AnimationCurve lifetimeCurve;

    void Awake()
    {
        effect = GetComponent<ParticleSystem>();
        mainModule = effect.main;
        emissionModule = effect.emission;
    }

    void Update()
    {
        float var = Mathf.Max(rateCurve.Evaluate(Mathf.Clamp(rigid.velocity.magnitude * 0.001f, 0f, 1000f)) * 1000f - 0.1f, 0f);
        emissionModule.rateOverTime = var;
        mainModule.startLifetime = lifetimeCurve.Evaluate(Mathf.Clamp(var.LinearRemap(10f, 300f, 0f, 1f), 0f, 1f));
    }
}
