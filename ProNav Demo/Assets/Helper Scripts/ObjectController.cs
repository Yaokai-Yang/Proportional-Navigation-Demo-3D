using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public float speed = 5;
    public float maneuverability = 5;
    public Quaternion targetRotation;

    void FixedUpdate()
    {
        // moves object forward by speed
        transform.localPosition += transform.localRotation * Vector3.forward * speed * Time.fixedDeltaTime;
        
        // rotates object towards a direction set by other algorithms
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, maneuverability * Time.deltaTime);
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.black);
        Debug.DrawRay(transform.position, targetRotation * Vector3.forward, Color.white);
    }
}
