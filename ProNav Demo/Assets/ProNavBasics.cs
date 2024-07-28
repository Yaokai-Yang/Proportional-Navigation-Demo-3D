using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProNavBasics : MonoBehaviour
{
    public ObjectController controller;
    public Transform target;
    public float proportionality_constant;      // variable N in the guidance law
        
    public Vector3 last_LOS;                    // LOS is calculated as a position vector from the target to the pursuer
    public Vector3 current_LOS;

    // returns a delta LOS as a float in degrees and closing velocity as a 3-vector
    protected (float, float) getDeltaLOS()
    {
        current_LOS = target.position - transform.position;                                 // get current LOS
        float delta_LOS = Vector3.SignedAngle(last_LOS, current_LOS, Vector3.right);        // compare with previous LOS to get change in LOS (only works in a single plane for now) 
        float closing_velocity = -(current_LOS.magnitude - last_LOS.magnitude);
        last_LOS = current_LOS;                                                             // update last_LOS for next iteration

        return (delta_LOS, closing_velocity);
    }

    protected abstract Vector3 doUpdate(float deltaTime);

    void Awake()
    {
        last_LOS = target.position - transform.position;
    }

    void FixedUpdate()
    {
        controller.targetVelocity = doUpdate(Time.deltaTime);
    }
}
