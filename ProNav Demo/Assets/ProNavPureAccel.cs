using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProNavPureAccel : ProNavBasics
{
    
    protected override Vector3 doUpdate(float deltaTime)
    {
        (float, float) delta = getDeltaLOS();
        float delta_LOS = delta.Item1;

        // acceleration_pure = N * change_in_LOS_angle * velocity
        Vector3 perpendicular = Quaternion.AngleAxis(90, Vector3.right) * controller.velocity.normalized;       // get direction perpendicular to velocity vector
        Vector3 acceleration = perpendicular * (proportionality_constant * delta_LOS * controller.velocity.magnitude) * deltaTime;

        return controller.velocity + acceleration;
    }
}
