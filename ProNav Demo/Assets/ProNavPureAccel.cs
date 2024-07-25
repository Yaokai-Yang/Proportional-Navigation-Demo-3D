using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProNavPureAccel : ProNavBasics
{
    // acceleration_pure = N * change_in_LOS_angle * velocity
    protected override Vector3 doUpdate(float deltaTime)
    {
        float deltaLOS = getDeltaLOS();
        Vector3 perpendicular = Quaternion.AngleAxis(90, Vector3.right) * controller.velocity.normalized;          // get direction perpendicular to velocity vector
        return controller.velocity + perpendicular * (proportionality_constant * deltaLOS * controller.velocity.magnitude) * deltaTime;
    }
}
