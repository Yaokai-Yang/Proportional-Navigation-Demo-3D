using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ZeroEffortMiss : ProNavBasics
{
    // acceleration = N * ZEM_perp / (time_to_go ^ 2)
    // gives acceleration commands perpendicular to the line of sight based on the 'Zero Effort Miss vector' and an estimated time until collision
    // the above equation (with an assumed time_to_go) simplifies to the proportional navigation law: acceleration = N * closing_velocity * LOS_rate [Zarchan, 2012, pg.32-33]
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

        Vector3 acceleration = proportionality_constant * ZEM_normal / (time_to_go * time_to_go);
        return controller.velocity + acceleration;
    }
}
