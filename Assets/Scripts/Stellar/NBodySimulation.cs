using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NBodySimulation : MonoBehaviour
{
    private CelestialBody[] bodies;
    public float timeScale = 1f;

    private void Awake()
    {
        bodies = FindObjectsOfType<CelestialBody>();
        Time.fixedDeltaTime = Universe.physicsTimeStep;
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
