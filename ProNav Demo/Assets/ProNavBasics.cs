using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProNavBasics : MonoBehaviour
{
    public ObjectController controller;
    public Transform target;
    public float proportionality_constant;      // variable N in the guidance law
        
    private Vector3 last_LOS;                   // LOS is calculated as a position vector from the target to the pursuer
    private Vector3 current_LOS;

    // returns delta LOS as a quaternion
    protected Quaternion getDeltaLOS()
    {
        current_LOS = target.position - transform.position;                         // get current LOS
        Quaternion delta_LOS = Quaternion.FromToRotation(last_LOS, current_LOS);    // compare with previous LOS to get change in LOS
        last_LOS = current_LOS;                                                     // update last_LOS for next iteration

        return delta_LOS;
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
