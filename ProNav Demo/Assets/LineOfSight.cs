using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LineOfSight : ProNavBasics
{
    // acceleration = N * closing_velocity * LOS_rate

    private Vector3 last_LOS;                   // LOS is calculated as a position vector from the target to the pursuer
    private Vector3 current_LOS;

    // returns delta LOS as a quaternion and the closing velocity as a float
    protected (Quaternion, float) getDelta(float deltaTime)
    {
        current_LOS = target.position - transform.position;                         // get current LOS
        Quaternion delta_LOS = Quaternion.FromToRotation(last_LOS, current_LOS);    // compare with previous LOS to get change in LOS
        float closing_velocity = -(current_LOS.magnitude - last_LOS.magnitude) / deltaTime;
        last_LOS = current_LOS;                                                     // update last_LOS for next iteration

        return (delta_LOS, closing_velocity);
    }

    protected override Vector3 doUpdate(float deltaTime)
    {
        (Quaternion delta_LOS, float closing_velocity) = getDelta(deltaTime);
        float LOS_rate = Quaternion.Angle(Quaternion.identity, delta_LOS) * Mathf.Deg2Rad / deltaTime;

        Vector3 desired_acceleration = (delta_LOS * controller.velocity) - controller.velocity;
        Vector3 acceleration_norm = Vector3.ProjectOnPlane(desired_acceleration, current_LOS).normalized;
        Vector3 acceleration = acceleration_norm * proportionality_constant * closing_velocity * LOS_rate;

        Debug.DrawRay(transform.position, acceleration * 5);

        return controller.velocity + acceleration;
    }

    void Awake()
    {
        last_LOS = target.position - transform.position;
    }
}
