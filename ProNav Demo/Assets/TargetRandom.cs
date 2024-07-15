using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class TargetRandom: MonoBehaviour
{
    // picks a direction, rotates towards it, picks a new direction once it has reached its previous direction
    public ObjectController controller;

    public void Start()
    {
        controller.targetRotation = Random.rotationUniform;
    }
    public void FixedUpdate()
    {
        if (transform.localRotation == controller.targetRotation)
        {
            controller.targetRotation = Random.rotationUniform;
        }
    }
}
