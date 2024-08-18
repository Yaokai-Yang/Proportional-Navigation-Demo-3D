using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public float sensitivity;
    private Vector2 rotation = new Vector2(0, -180);

    public float max_horizontal_speed;
    private Vector2 horizontal_speed = Vector2.zero;

    public float max_vertical_speed;
    private float vertical_speed = 0;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        transform.LookAt(target);
    }

    private void Update()
    {
        rotation.y += Input.GetAxis("mouse_x");
        rotation.x += -Input.GetAxis("mouse_y");
        rotation.x = Mathf.Clamp(rotation.x, -180, 180);
        transform.eulerAngles = (Vector2)rotation * sensitivity;

        horizontal_speed.x = Input.GetAxis("x_movement");
        horizontal_speed.y = Input.GetAxis("z_movement");
        transform.localPosition += transform.localRotation * new Vector3(horizontal_speed.x, 0, horizontal_speed.y) * max_horizontal_speed * Time.deltaTime;

        vertical_speed = Input.GetAxis("y_movement");
        transform.localPosition += new Vector3(0, vertical_speed, 0) * max_vertical_speed * Time.deltaTime;
    }
}
