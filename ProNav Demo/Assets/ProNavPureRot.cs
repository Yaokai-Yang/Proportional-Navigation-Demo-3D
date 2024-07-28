using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProNavPureRot : ProNavBasics
{
    protected override Vector3 doUpdate(float deltaTime)
    {
        (float, float) delta = getDeltaLOS();
        float delta_LOS = delta.Item1;

        // Change_in_flight_angle = N * change_in_LOS_angle
        Quaternion to_rotate = Quaternion.AngleAxis(delta_LOS * proportionality_constant, Vector3.right);   // Vector3.right is used as a placeholder until 3D is added
        return to_rotate * controller.velocity;                                                             // rotate current velocity by a factor of deltaLOS
    }
}
