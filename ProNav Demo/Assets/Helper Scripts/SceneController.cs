using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Input = UnityEngine.Input;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public bool paused;
    public bool reset;
    public GameObject resume_icon, pause_icon;
    public GameObject mouse_icon, mouse_grey_icon;

    public bool mouselook;
    public CameraController main_camera;
    public TrackingCamera tracking_camera;

    public GameObject starting_conditions_UI;
    public TextMeshProUGUI tracking_camera_UI;
    public TextMeshProUGUI active_pursuer_UI;
    public GameObject pronav_constant_UI;

    public TMP_InputField[] starting_conditions = new TMP_InputField[9];        // indicies 0-2: position, 3-5: velocity, 6: max rotation, 7: max acceleration, 8: proportionality constant (N)
    private bool editing;               // prevent updating text values when editing them through the UI

    private ObjectController[] controllers;
    private int active_idx;

    public void Awake()
    {
        editing = false;

        findObjects();
        active_idx = controllers.Length - 1;
        resetScene();

        mouselook = true;
        Cursor.lockState = CursorLockMode.Locked;
        main_camera.mouselook = true;
    }

    private void findObjects()
    {
        GameObject[] pursuers = GameObject.FindGameObjectsWithTag("Pursuer");
        controllers = new ObjectController[pursuers.Length + 1];
        for (int i = 0; i < pursuers.Length; i++)
        {
            controllers[i] = pursuers[i].GetComponent<ObjectController>();
        }
        controllers[pursuers.Length] = GameObject.Find("Target").GetComponent<ObjectController>();
    }

    private void resetScene()
    {
        reset = true;
        starting_conditions_UI.SetActive(true);

        foreach (ObjectController controller in controllers)
        {
            controller.onSceneReset();
        }

        pauseScene();       // also sets 'paused' to 'true'
    }

    private void pauseScene()
    {
        paused = true;
        resume_icon.SetActive(false);
        pause_icon.SetActive(true);

        foreach (ObjectController controller in controllers)
        {
            controller.onPause();
        }
    }

    private void resumeScene()
    {
        paused = false;
        reset = false;
        resume_icon.SetActive(true);
        pause_icon.SetActive(false);
        starting_conditions_UI.SetActive(false);

        foreach (ObjectController controller in controllers)
        {
            controller.onUnpause();
        }
    }

    public void nextObject()
    {
        active_idx++;
        if (active_idx >= controllers.Length)
        {
            active_idx = 0;
        }
    }

    public void prevObject()
    {
        active_idx--;
        if (active_idx <= -1)
        {
            active_idx = controllers.Length - 1;
        }
    }

    // The next 3 functions are to do with the input fields for starting conditions
    public void startEdit()
    {
        editing = true;
    }

    public void stopEdit()
    {
        editing = false;
        updateStartingConditions();     // doing it this way instead of in 'onValueChanged' prevents this function from calling after switching the active object
    }

    public void updateStartingConditions()
    {
        for (int i = 0; i < starting_conditions.Length; i++)
        {
            if (starting_conditions[i].text == "")
            {
                return;
            }
        }

        // update controller variables
        ObjectController controller = controllers[active_idx];
        Vector3 starting_pos = new Vector3(float.Parse(starting_conditions[0].text), float.Parse(starting_conditions[1].text), float.Parse(starting_conditions[2].text));
        Vector3 starting_vel = new Vector3(float.Parse(starting_conditions[3].text), float.Parse(starting_conditions[4].text), float.Parse(starting_conditions[5].text));
        controller.gameObject.transform.position = starting_pos;
        controller.starting_position = starting_pos;
        controller.velocity = starting_vel;
        controller.starting_velocity = starting_vel;

        controller.maneuverability_rot = float.Parse(starting_conditions[6].text);
        controller.maneuverability_accel = float.Parse(starting_conditions[7].text);

        // update pronav constant for objects that use it (could probably be made to look nicer)
        if (controller.gameObject.GetComponent<LineOfSight>())
        {
            controller.gameObject.GetComponent<LineOfSight>().proportionality_constant = int.Parse(starting_conditions[8].text);
        }
        else if (controller.gameObject.GetComponent<ZeroEffortMiss>())
        {
            controller.gameObject.GetComponent<ZeroEffortMiss>().proportionality_constant = int.Parse(starting_conditions[8].text);
        }
    }

    private void Update()
    {
        // reset scenario
        if (Input.GetButtonDown("reset") && reset == false)
        {
            resetScene();
        }

        // pause and resume scenario
        if (Input.GetButtonDown("pause"))
        {
            if (paused)
            {
                resumeScene();
            }
            else
            {
                pauseScene();
            }
        }

        // switching between mouselook and interacting with UI
        if (Input.GetButtonDown("esc"))
        {
            if (mouselook)
            {
                mouselook = false;
                Cursor.lockState = CursorLockMode.None;
                main_camera.mouselook = false;

                mouse_icon.SetActive(false);
                mouse_grey_icon.SetActive(true);
            }
            else
            {
                mouselook = true;
                Cursor.lockState = CursorLockMode.Locked;
                main_camera.mouselook = true;

                mouse_icon.SetActive(true);
                mouse_grey_icon.SetActive(false);
            }
        }

        // during mouselook, left and right click cycles through the targeted pursuer for the tracking camera
        if (controllers.Length > 1)
        {
            // left mouse cycles forward
            if (Input.GetMouseButtonDown(0) && mouselook)
            {
                nextObject();
            }

            // right mouse cycles backward
            if (Input.GetMouseButtonUp(1) && mouselook)
            {
                prevObject();
            }

            tracking_camera.target = controllers[active_idx].transform;
            tracking_camera_UI.text = "Looking at: " + controllers[active_idx].gameObject.name;
            active_pursuer_UI.text = "Active Object: " + controllers[active_idx].gameObject.name;
        }
        else
        {
            tracking_camera_UI.text = "No Objects";
            active_pursuer_UI.text = "No Objects";
        }

        // input fields UI
        if (!editing)
        {
            for (int i = 0; i < starting_conditions.Length; i++)
            {
                if (starting_conditions[i].text == "")
                {
                    starting_conditions[i].text = "0";
                }
            }

            ObjectController active_controller = controllers[active_idx];

            // 0 - 2: position, 3 - 5: velocity, 6: max rotation, 7: max acceleration, 8 proportionality constant (N)
            starting_conditions[0].text = active_controller.starting_position.x.ToString();
            starting_conditions[1].text = active_controller.starting_position.y.ToString();
            starting_conditions[2].text = active_controller.starting_position.z.ToString();

            starting_conditions[3].text = active_controller.starting_velocity.x.ToString();
            starting_conditions[4].text = active_controller.starting_velocity.y.ToString();
            starting_conditions[5].text = active_controller.starting_velocity.z.ToString();

            starting_conditions[6].text = active_controller.maneuverability_rot.ToString();
            starting_conditions[7].text = active_controller.maneuverability_accel.ToString();

            pronav_constant_UI.SetActive(true);
            if (active_controller.gameObject.GetComponent<LineOfSight>())
            {
                starting_conditions[8].text = active_controller.gameObject.GetComponent<LineOfSight>().proportionality_constant.ToString();
            }
            else if (active_controller.gameObject.GetComponent<ZeroEffortMiss>())
            {
                starting_conditions[8].text = active_controller.gameObject.GetComponent<ZeroEffortMiss>().proportionality_constant.ToString();
            }
            else
            {
                pronav_constant_UI.SetActive(false);
            }
        }
    }
}
