using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            paused = togglePause();
        }
    }

    void OnGUI()
    {
        if (!paused)
        {
            gameObject.GetComponentInChildren<Canvas>().enabled = false;
        }
        if (paused)
        {
            gameObject.GetComponentInChildren<Canvas>().enabled = true;
            GUILayout.Label("Game is paused!");
            if (GUILayout.Button("Click me to unpause"))
                paused = togglePause();
            if (GUILayout.Button("Click me to unpause"))
                paused = togglePause();
        }
    }

    public bool togglePause()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            return (false);
        }
        else
        {
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            return (true);
        }
    }
}
