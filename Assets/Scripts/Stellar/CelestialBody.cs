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
    private Transform meshHolder;

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
        meshHolder = transform.GetChild(0);
        meshHolder.localPosition = Vector3.zero;
        meshHolder.localScale = Vector3.one * radius;
        List<Material> mats = new List<Material>();
        meshHolder.GetComponent<MeshRenderer>().GetMaterials(mats);
        mats[0].color = mainColor;
    }

    public void UpdateVelocity(CelestialBody[] celestialBodies, float timeStep)
    {
        velocity = CalculateVelocity(this, celestialBodies, timeStep);
    }

    public void UpdatePosition(float timeStep)
    {
        rigid.position = CalculatePosition(this, timeStep);
    }

    public static Vector3 CalculateVelocity(IGravitationalBody body, IGravitationalBody[] celestialBodies, float timeStep)
    {
        Vector3 velocity = body.velocity;
        foreach (var otherBody in celestialBodies)
        {
            if (otherBody != body)
            {
                float sqrDst = (otherBody.position - body.position).sqrMagnitude;
                Vector3 forceDir = (otherBody.position - body.position).normalized;
                Vector3 force = forceDir * Universe.gravitationalConstant * body.mass * otherBody.mass / sqrDst;
                Vector3 acceleration = force / body.mass;
                velocity += acceleration * timeStep;
            }
        }
        return velocity;
    }

    public static Vector3 CalculatePosition(IGravitationalBody body, float timeStep)
    {
        return body.position + body.velocity * timeStep;
    }
}
