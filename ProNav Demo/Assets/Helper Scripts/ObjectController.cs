using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public float maneuverability_rot;            // max anuglar acceleration in PI*radians per second
    public float maneuverability_accel;          // max acceleration in units per second^2
    public Vector3 target_velocity;
    public Vector3 velocity;

    public Vector3 starting_velocity;
    public Vector3 starting_position;

    public bool paused;
    public bool reset;

    public TrailRenderer trail;
    private float trail_time = 2;
    private float paused_trail_time = 2;
    private float timer = 0;

    public void onSceneReset()
    {
        reset = true;
        transform.position = starting_position;
        velocity = starting_velocity;
        target_velocity = starting_velocity;

        trail.Clear();
    }

    public void onPause()
    {
        paused = true;

        trail.time = trail_time;
    }

    public void onUnpause()
    {
        paused = false;
        reset = false;

        timer = 0;
        paused_trail_time = trail.time;
    }

    private void FixedUpdate()
    {
        if (!paused)
        {
            // moves object forward by speed
            transform.localPosition += velocity * Time.fixedDeltaTime;

            // tries to achieve the target velocity via rotation & acceleration
            velocity = Vector3.RotateTowards(velocity, target_velocity, maneuverability_rot * Mathf.PI * Time.fixedDeltaTime, maneuverability_accel * Time.fixedDeltaTime);

            // trail length rendering
            timer += Time.deltaTime;
            trail.time = Mathf.Lerp(paused_trail_time, trail_time, Mathf.Clamp(timer / trail_time, 0f, 1f));
        }
        else if (!reset)
        {
            trail.time += Time.deltaTime;
        }
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, target_velocity, Color.black);
        Debug.DrawRay(transform.position, velocity, Color.grey);
    }
}
