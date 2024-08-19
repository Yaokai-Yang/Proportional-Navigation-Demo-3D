using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public bool paused;
    public bool reset;
    public GameObject resume_icon, pause_icon;

    private ObjectController[] controllers;
    public void Awake()
    {
        findObjects();
        resetScene();
    }

    private void findObjects()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Pursuer");
        controllers = new ObjectController[objects.Length + 1];
        for (int i = 0; i < objects.Length; i++)
        {
            controllers[i] = objects[i].GetComponent<ObjectController>();
        }
        controllers[objects.Length] = GameObject.Find("Target").GetComponent<ObjectController>();
    }

    private void resetScene()
    {
        reset = true;

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

        foreach (ObjectController controller in controllers)
        {
            controller.onUnpause();
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
    }
}
