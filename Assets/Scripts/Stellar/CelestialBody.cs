using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Rigidbody))]
public class CelestialBody : MonoBehaviour, IGravitationalBody
{
    public float radius;
    public float surfaceGravity;
    public Vector3 initialVelocity;
    public float mass { get; protected set; }
    public Vector3 velocity { get; protected set; }
    public Vector3 position
    {
        get
        {
            return rigid.position;
        }
        set
        {
            rigid.position = value;
        }
    }
    public Color mainColor
    {
        get
        {
            return color;
        }
    }
    [SerializeField]
    private Color color = Color.white;

    private Rigidbody rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.isKinematic = true;
        rigid.mass = mass;
        velocity = initialVelocity;
    }

    void OnValidate()
    {
        mass = surfaceGravity * radius * radius / Universe.gravitationalConstant;
        Planet planet = transform.GetComponent<Planet>();
        if (planet != null)
        {
            planet.shapeSettings.radius = radius;
        }
    }

    public void UpdateVelocity(CelestialBody[] celestialBodies, float timeStep)
    {
        Vector3 newVelocity = Vector3.zero;
        foreach (CelestialBody otherBody in celestialBodies)
        {
            if (otherBody as Object != this)
            {
                float sqrDst = (otherBody.position - position).sqrMagnitude;
                Vector3 forceDir = (otherBody.position - position).normalized;
                Vector3 force = forceDir * Universe.gravitationalConstant * mass * otherBody.mass / sqrDst;
                Vector3 acceleration = force / mass;
                newVelocity += acceleration * timeStep;
            }
        }
        velocity += newVelocity;
    }

    public void UpdatePosition(float timeStep)
    {
        position += velocity * timeStep;
    }
}
