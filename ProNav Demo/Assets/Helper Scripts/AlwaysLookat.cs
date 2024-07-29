using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysLookat : MonoBehaviour
{
    // Naive intercept, always tries to point at where the target is
    public ObjectController controller;
    public Transform target;

    void Update()
    {
        // rotates the velocity vector to the direction-to-target
        controller.targetVelocity = Vector3.RotateTowards(controller.targetVelocity, target.position - transform.position, 2*Mathf.PI, 0);
    }
}
