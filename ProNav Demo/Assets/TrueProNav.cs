using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrueProNav : MonoBehaviour
{
    public ObjectController controller;
    public Transform target;
    public int proportionality_constant;      // variable N in the guidance law

    public Vector3 last_LOS;                   // LOS is calculated as a position vector from the target to the pursuer
    public Vector3 current_LOS;
    public Quaternion delta_LOS;

    public void Start()
    {
        last_LOS = target.position - transform.position;
    }

    public void FixedUpdate()
    {
        current_LOS = target.position - transform.position;                 // get current LOS
        delta_LOS = Quaternion.FromToRotation(last_LOS, current_LOS);       // compare with previous LOS to get change in LOS as a rotation vector

        Quaternion to_rotate = Quaternion.identity;
        for (int i = 0; i < proportionality_constant; i++)
        {
            to_rotate *= delta_LOS;
        }
        controller.targetRotation = transform.rotation * to_rotate;

        last_LOS = current_LOS;                                             // update last_LOS for next iteration
    }
}
