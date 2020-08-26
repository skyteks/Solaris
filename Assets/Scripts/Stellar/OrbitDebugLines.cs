using UnityEngine;

[ExecuteInEditMode]
public class OrbitDebugLines : MonoBehaviour
{
    private class VirtualBody : IGravitationalBody
    {
        public Vector3 position { get; set; }
        public Vector3 velocity { get; set; }
        public float mass { get; private set; }
        public Color mainColor { get; private set; }

        public VirtualBody(CelestialBody body)
        {
            position = body.position;
            velocity = body.initialVelocity;
            mass = body.mass;
            mainColor = body.mainColor;
        }
    }

    public int timeSteps;

    void Start()
    {
        if (Application.isPlaying)
        {
        }
    }

    void LateUpdate()
    {
        if (!Application.isPlaying)
        {
            DrawOrbits();
        }
    }

    private void DrawOrbits()
    {
        CelestialBody[] bodies = FindObjectsOfType<CelestialBody>();
        VirtualBody[] virtualBodies = new VirtualBody[bodies.Length];
        for (int i = 0; i < bodies.Length; i++)
        {
            virtualBodies[i] = new VirtualBody(bodies[i]);
        }

        for (int step = 0; step < timeSteps; step++)
        {
            foreach (var body in virtualBodies)
            {
                body.velocity = CelestialBody.CalculateVelocity(body, virtualBodies, step * Universe.physicsTimeStep);
            }
            foreach (var body in virtualBodies)
            {
                Vector3 nextPos = CelestialBody.CalculatePosition(body, step * Universe.physicsTimeStep);
                Debug.DrawLine(body.position, nextPos, body.mainColor);
                body.position = nextPos;
            }
        }
    }
}
