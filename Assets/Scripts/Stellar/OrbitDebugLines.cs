using UnityEngine;

[ExecuteInEditMode]
public class OrbitDebugLines : MonoBehaviour
{
    private class VirtualBody : IGravitationalBody
    {
        public Vector3 position { get; private set; }
        public Vector3 lastPosition { get; private set; }
        public Vector3 velocity { get; set; }
        public float mass { get; private set; }
        public float radius { get; private set; }
        public Color mainColor { get; private set; }

        public void SetNewPosition(Vector3 newPos)
        {
            lastPosition = position;
            position = newPos;
        }

        public VirtualBody(CelestialBody body)
        {
            position = body.position;
            velocity = body.initialVelocity;
            mass = body.mass;
            radius = body.radius;
            mainColor = body.mainColor;
        }
    }

    public int timeSteps;
    public bool useRelativeObject;
    public CelestialBody relativeObject;
    private VirtualBody relativeVirtual;
    public Mesh sphereMesh;

    void OnDrawGizmos()
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
        relativeVirtual = null;
        for (int i = 0; i < bodies.Length; i++)
        {
            virtualBodies[i] = new VirtualBody(bodies[i]);
            if (useRelativeObject && relativeObject != null && bodies[i] == relativeObject)
            {
                relativeVirtual = virtualBodies[i];
            }
        }

        bool collision = false;
        for (int step = 0; step < timeSteps; step++)
        {
            if (collision)
            {
                break;
            }
            foreach (var body in virtualBodies)
            {
                body.velocity = CelestialBody.CalculateVelocity(body, virtualBodies, 1f);
            }
            foreach (var body in virtualBodies)
            {
                body.SetNewPosition(CelestialBody.CalculatePosition(body, 1f));
            }
            foreach (var body in virtualBodies)
            {
                foreach (var otherBody in virtualBodies)
                {
                    if (body == otherBody)
                    {
                        continue;
                    }
                    if (Vector3.Distance(body.position, otherBody.position) < body.radius + otherBody.radius)
                    {
                        Gizmos.color = body.mainColor;
                        //Gizmos.DrawSphere(body.position, body.radius);
                        Gizmos.DrawMesh(sphereMesh, body.position, Quaternion.identity, Vector3.one * body.radius);
                        Gizmos.color = otherBody.mainColor;
                        Gizmos.DrawMesh(sphereMesh, otherBody.position, Quaternion.identity, Vector3.one * otherBody.radius);
                        collision = true;
                    }
                }

                Gizmos.color = body.mainColor;
                if (useRelativeObject && relativeVirtual != null)
                {
                    Gizmos.DrawLine(body.lastPosition - relativeVirtual.position, body.position - relativeVirtual.position);
                }
                else
                {
                    Gizmos.DrawLine(body.lastPosition, body.position);
                }
            }
        }
    }
}
