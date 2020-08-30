using System.Collections.Generic;
using UnityEngine;

public class EndlessManager : MonoBehaviour
{
    public float distanceThreshold = 1000f;
    private List<Rigidbody> physicsObjects;

    public event System.Action PostFloatingOriginUpdate;

    void Awake()
    {
        physicsObjects = new List<Rigidbody>(FindObjectsOfType<Rigidbody>());
    }

    void LateUpdate()
    {
        UpdateFloatingOrigin();
        PostFloatingOriginUpdate?.Invoke();
    }

    void UpdateFloatingOrigin()
    {
        Vector3 originOffset = Camera.main.transform.position;
        float distanceFromOrigin = originOffset.magnitude;

        if (distanceFromOrigin > distanceThreshold)
        {
            foreach (Rigidbody rigid in physicsObjects)
            {
                rigid.position -= originOffset;
            }
        }
    }
}