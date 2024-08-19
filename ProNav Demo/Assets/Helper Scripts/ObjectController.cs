using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public float maneuverabilityRot;            // max anuglar acceleration in PI*radians per second
    public float maneuverabilityAccel;          // max acceleration in units per second^2
    public Vector3 targetVelocity;
    public Vector3 velocity;   

    private void Awake()
    {
        velocity = targetVelocity;
    }

    private void FixedUpdate()
    {
        // moves object forward by speed
        transform.localPosition += velocity * Time.fixedDeltaTime;

        // tries to achieve the target velocity via rotation & acceleration
        velocity = Vector3.RotateTowards(velocity, targetVelocity, maneuverabilityRot * Mathf.PI * Time.fixedDeltaTime, maneuverabilityAccel * Time.fixedDeltaTime);
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, targetVelocity, Color.black);
        Debug.DrawRay(transform.position, velocity, Color.grey);
    }
}
