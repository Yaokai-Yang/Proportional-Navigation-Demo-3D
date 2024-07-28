using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class ProNavTrue : ProNavBasics
{
    
    protected override Vector3 doUpdate(float deltaTime)
    {
        (float, float) delta = getDeltaLOS();
        float delta_LOS = delta.Item1;
        float closing_velocity = delta.Item2;

        // acceleration_true = N * change_in_LOS_angle * closing_velocity
        // acceleration_true is along the axis perpendicular to the LOS
        Vector3 LOS = target.position - transform.position;
        Vector3 perpendicular = Quaternion.AngleAxis(90, Vector3.right) * LOS;     
        Vector3 acceleration_true = perpendicular * (proportionality_constant * delta_LOS * closing_velocity) * deltaTime;

        return controller.velocity + acceleration_true;
    }
}
