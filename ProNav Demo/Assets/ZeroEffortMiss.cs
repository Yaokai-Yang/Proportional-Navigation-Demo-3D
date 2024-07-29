using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ZeroEffortMiss : ProNavBasics
{
    protected override Vector3 doUpdate(float deltaTime)
    {
        // time_to_go -= deltaTime;

        Vector3 relative_position = target.position - transform.position;
        Vector3 relative_velocity = target.GetComponent<ObjectController>().velocity - controller.velocity;

        float   time_to_go = relative_position.magnitude / relative_velocity.magnitude;
        Vector3 zero_effort_miss = relative_position + relative_velocity * time_to_go;

        Vector3 LOS_unit = relative_position.normalized;                        // unit vector towards the target
        Vector3 ZEM_proj_LOS = Vector3.Project(zero_effort_miss, LOS_unit);     // ZEM projected onto LOS
        Vector3 ZEM_normal = zero_effort_miss - ZEM_proj_LOS;                   // ZEM normal to LOS

        Vector3 acceleration = proportionality_constant * ZEM_normal / (time_to_go * time_to_go) * deltaTime;
        return controller.velocity + acceleration;
    }
}
