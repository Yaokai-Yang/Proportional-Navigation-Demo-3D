using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProNavPureRot : ProNavBasics
{
    // Change_in_flight_angle = N * change_in_LOS_angle
    protected override Vector3 doUpdate(float deltaTime)
    {
        float deltaLOS = getDeltaLOS();
        Quaternion toRotate = Quaternion.AngleAxis(deltaLOS * proportionality_constant, Vector3.right);     // Vector3.right is used as a placeholder until 3D is added
        return toRotate * controller.velocity;                                                              // rotate current velocity by a factor of deltaLOS
    }
}
