using UnityEngine;

public interface IGravitationalBody
{
    float mass { get; }
    Vector3 velocity { get; }
    Vector3 position { get; }
    Color mainColor { get; }
}
