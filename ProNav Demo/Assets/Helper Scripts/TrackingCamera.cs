using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class TrackingCamera : MonoBehaviour
{
    public Transform target;
    public Transform host;
    public float range = 15;

    private GameObject[] targets_list;
    private int current_target = 0;
    public TextMeshProUGUI target_UI;

    private void Awake()
    {
        getTargets();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Click");
            current_target++;
            if (current_target >= targets_list.Length)
            {
                current_target = 0;
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            current_target--;
            if (current_target <= -1)
            {
                current_target = targets_list.Length - 1;
            }
            
        }

        target = targets_list[current_target].transform;
        target_UI.text = "Looking at: " + target.gameObject.name;

        Vector3 to_target = (host.position - target.position).normalized * range;
        Vector3 target_position = host.position + to_target + (Vector3.up * range / 5);
        transform.position = target_position;
        transform.LookAt(target);
    }

    private void getTargets()
    {
        targets_list = GameObject.FindGameObjectsWithTag("Pursuer");
    }
}
