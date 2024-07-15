using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurePursuit : MonoBehaviour
{
    // Naive intercept, always tries to point at where the target is
    public ObjectController controller;
    public Transform target;

    void Update()
    {
        controller.targetRotation = Quaternion.LookRotation(target.position - transform.position);
    }
}
