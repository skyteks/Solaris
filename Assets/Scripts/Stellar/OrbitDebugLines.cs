using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class OrbitDebugLines : MonoBehaviour
{
    private class VirtualBody : IGravitationalBody
    {
        public string name { get; private set; }
        public Color mainColor { get; private set; }
        public Vector3 position { get; private set; }
        public Vector3 lastPosition { get; private set; }
        public Vector3 initialPosition { get; }
        public Vector3 velocity { get; set; }
        public float mass { get; private set; }
        public float radius { get; }
        public VirtualBody parent;

        public void SetNewPosition(Vector3 newPos)
        {
            lastPosition = position;
            position = newPos;
        }

        public VirtualBody(CelestialBody body)
        {
            name = body.name;
            position = body.position;
            initialPosition = body.position;
            velocity = body.initialVelocity;
            mass = body.mass;
            radius = body.radius;
            mainColor = body.mainColor;
        }
    }

    public enum RelativeTo
    {
        nothing,
        mostMassive,
        selected,
        strongestAcceleration,
    }

    public int steps = 4000;
    public float timeStep = 1f;
    public RelativeTo useRelativeObject;
    public CelestialBody relativeToObject;
    private VirtualBody relativeObject;
    private KeyValuePair<VirtualBody, VirtualBody>? collision;

    void Update()
    {
        if (!Application.isPlaying)
        {
            DrawOrbits();
        }
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            if (collision.HasValue)
            {
                DrawCollision(collision.Value.Key, collision.Value.Value);
            }
        }
    }

    void OnValidate()
    {
        if (useRelativeObject == RelativeTo.selected && relativeToObject == null)
        {
            useRelativeObject = RelativeTo.nothing;
        }
    }
    private void DrawOrbits()
    {
        collision = null;
        CelestialBody[] bodies = FindObjectsOfType<CelestialBody>();
        relativeObject = null;
        VirtualBody[] virtualBodies = new VirtualBody[bodies.Length];
        for (int i = 0; i < bodies.Length; i++)
        {
            virtualBodies[i] = new VirtualBody(bodies[i]);
            if (useRelativeObject == RelativeTo.selected && relativeToObject == bodies[i])
            {
                relativeObject = virtualBodies[i];
            }
            if ((useRelativeObject == RelativeTo.mostMassive || useRelativeObject == RelativeTo.strongestAcceleration) && (relativeObject == null || virtualBodies[i].mass > relativeObject.mass))
            {
                relativeObject = virtualBodies[i];
                relativeToObject = bodies[i];
            }
        }

        for (int step = 0; step < steps; step++)
        {
            if (collision.HasValue)
            {
                break;
            }
            foreach (var body in virtualBodies)
            {
                bool useStrongest = useRelativeObject == RelativeTo.strongestAcceleration;
                body.velocity += CalculateAcceleration(body, virtualBodies, 1f) * timeStep;
            }
            foreach (var body in virtualBodies)
            {
                body.SetNewPosition(body.position + body.velocity * timeStep);
            }

            foreach (var body in virtualBodies)
            {
                foreach (var otherBody in virtualBodies)
                {
                    if (body == otherBody)
                    {
                        continue;
                    }
                    float distance = Vector3.Distance(body.position, otherBody.position);
                    if (distance < body.radius + otherBody.radius)
                    {
                        collision = new KeyValuePair<VirtualBody, VirtualBody>(body, otherBody);
                    }
                }
                DrawOrbit(body);
            }
        }
    }

    private void DrawOrbit(VirtualBody body)
    {
        switch (useRelativeObject)
        {
            case RelativeTo.nothing:
                Debug.DrawLine(body.lastPosition, body.position, body.mainColor);
                break;
            case RelativeTo.strongestAcceleration:
                if (body == relativeObject)
                {
                    break;
                }
                Debug.DrawLine(body.lastPosition - (body.parent.lastPosition - body.parent.initialPosition), body.position - (body.parent.position - body.parent.initialPosition), body.mainColor);
                break;
            case RelativeTo.mostMassive:
            case RelativeTo.selected:
                if (body == relativeObject)
                {
                }
                Debug.DrawLine(body.lastPosition - (relativeObject.lastPosition - relativeObject.initialPosition), body.position - (relativeObject.position - relativeObject.initialPosition), body.mainColor);
                break;
            default:
                break;
        }
    }

    private void DrawCollision(VirtualBody body, VirtualBody otherBody)
    {
        Gizmos.color = body.mainColor;
        Gizmos.DrawSphere(body.position, body.radius);
        //Gizmos.DrawMesh(sphereMesh, body.position, Quaternion.identity, Vector3.one * body.radius);
        Gizmos.color = otherBody.mainColor;
        Gizmos.DrawSphere(otherBody.position, otherBody.radius);
        //Gizmos.DrawMesh(sphereMesh, otherBody.position, Quaternion.identity, Vector3.one * otherBody.radius);
    }

    private Vector3 CalculateAcceleration(VirtualBody body, VirtualBody[] celestialBodies, float timeStep, bool onlyStrongest = false)
    {
        Vector3 totalAcceleration = Vector3.zero;
        Vector3 strongestAcceleration = Vector3.zero;
        foreach (var otherBody in celestialBodies)
        {
            if (otherBody != body)
            {
                float sqrDst = (otherBody.position - body.position).sqrMagnitude;
                Vector3 forceDir = (otherBody.position - body.position).normalized;
                Vector3 acceleration = forceDir * Universe.gravitationalConstant * otherBody.mass / sqrDst;
                if (acceleration.sqrMagnitude > strongestAcceleration.sqrMagnitude && body != relativeObject)
                {
                    body.parent = otherBody;
                    strongestAcceleration = acceleration;
                }
                totalAcceleration += acceleration;
            }
        }
        return onlyStrongest ? strongestAcceleration : totalAcceleration;
    }
}
