using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class TrackingCamera : MonoBehaviour
{
    public Transform target;
    public Transform host;
    public float base_range = 15;
    private float range;

    private void Update()
    {
        if (target == host)
        {
            range = base_range * 3;
        }
        else
        {
            range = base_range;
        }

        Vector3 to_target = (host.position - target.position).normalized * range;
        Vector3 target_position = host.position + to_target + (Vector3.up * range / 5);
        transform.position = target_position;
        transform.LookAt(target);
    }
}
