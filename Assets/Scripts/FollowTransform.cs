using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    public enum UpdateStates
    {
        Update,
        LateUpdate,
    }

    public UpdateStates updateState;
    public Transform followTransform;
    public bool linkRotation;

    void Update()
    {
        switch (updateState)
        {
            case UpdateStates.Update:
                Follow();
                break;
            default:
                return;
        }
    }

    void LateUpdate()
    {
        switch (updateState)
        {
            case UpdateStates.LateUpdate:
                Follow();
                break;
            default:
                return;
        }
    }

    private void Follow()
    {
        transform.position = followTransform.position;
        if (linkRotation)
        {
            transform.rotation = followTransform.rotation;
        }
    }
}
