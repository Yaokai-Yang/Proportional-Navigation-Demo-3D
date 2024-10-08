using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProNavBasics : MonoBehaviour
{
    public ObjectController controller;
    public Transform target;
    public float proportionality_constant;      // variable N in the guidance law

    protected abstract Vector3 doUpdate(float deltaTime);

    private void FixedUpdate()
    {
        if (!controller.paused)
        {
            controller.target_velocity = doUpdate(Time.fixedDeltaTime);
        }
    }
}
