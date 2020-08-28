using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NBodySimulation : MonoBehaviour
{
    private CelestialBody[] bodies;
    [Range(0f, 70f)]
    public float timeScale = 1f;

    void Awake()
    {
        bodies = FindObjectsOfType<CelestialBody>();
        Time.fixedDeltaTime = Universe.physicsTimeStep;
    }

    void OnValidate()
    {
        Time.timeScale = timeScale;
    }

    void FixedUpdate()
    {
        foreach (var body in bodies)
        {
            body.UpdateVelocity(bodies, Universe.physicsTimeStep);
        }
        foreach (var body in bodies)
        {
            body.UpdatePosition(Universe.physicsTimeStep);
        }
    }
}
