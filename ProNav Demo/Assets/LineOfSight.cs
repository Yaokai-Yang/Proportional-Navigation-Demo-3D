using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LineOfSight : ProNavBasics
{
    // acceleration = N * closing_velocity * LOS_rate
    // gives acceleration commands perpendicular to the line of sight based on the closing velocity and change in LOS
    // Equation on [Zarchan, 2012, pg.14], adapted for 3D.

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
        float LOS_rate = Quaternion.Angle(Quaternion.identity, delta_LOS) / deltaTime * Mathf.Deg2Rad;

        // gets a unit vector orthogonal to the LOS in the direction to accelerate
        // the desired direction of travel (as a velocity) is described by the current velocity rotated by delta_LOS
        // the difference of the 'desired' velocity and current velocity, projected onto the plane whose normal is the LOS, gives the direction of acceleration
        Vector3 desired_velocity = (delta_LOS * controller.velocity) - controller.velocity;
        Vector3 acceleration_norm = Vector3.ProjectOnPlane(desired_velocity, current_LOS).normalized;

        Vector3 acceleration = acceleration_norm * Mathf.Abs(proportionality_constant * closing_velocity * LOS_rate);

        return controller.velocity + acceleration;
    }

    private void OnEnable()
    {
        last_LOS = target.position - transform.position;
    }
}
