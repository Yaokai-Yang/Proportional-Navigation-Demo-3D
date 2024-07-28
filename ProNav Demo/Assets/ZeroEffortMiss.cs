using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ZeroEffortMiss : ProNavBasics
{
    public float time_to_go;
    protected override Vector3 doUpdate(float deltaTime)
    {
        Vector3 relative_position = target.position - transform.position;
        Vector3 relative_velocity = target.GetComponent<ObjectController>().velocity - controller.velocity;
        Vector3 zero_effort_miss = relative_position + relative_velocity * time_to_go;

        Vector3 LOS_unit = relative_position.normalized;                        // unit vector towards the target
        Vector3 ZEM_proj_LOS = Vector3.Project(zero_effort_miss, LOS_unit); 
        Vector3 ZEM_normal = zero_effort_miss - ZEM_proj_LOS;

        Vector3 acceleration = proportionality_constant * ZEM_normal / (time_to_go * time_to_go) * deltaTime;
        return controller.velocity + acceleration;
    }
}
