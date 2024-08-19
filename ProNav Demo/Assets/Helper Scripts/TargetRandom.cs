using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Threading;
using UnityEngine;

public class TargetRandom: MonoBehaviour
{
    // randomly picks a new target velocity every time it reaches its current goal
    public ObjectController controller;

    public void Start()
    {
        // Vector2 rand = Random.insideUnitCircle.normalized * controller.targetVelocity.magnitude;
        // controller.targetVelocity = new Vector3(0, rand.x, rand.y);
    }
    public void FixedUpdate()
    {
        if ((controller.target_velocity - controller.velocity).magnitude < 0.05f)
        {
            Vector3 rand = Random.onUnitSphere * controller.target_velocity.magnitude;
            controller.target_velocity = rand;
        }
    }
}
