using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Texture2D cursor;
    public GameObject gameOver;
    public GameObject mainMenu;
    public GameObject winMenu;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mainMenu.GetComponent<Canvas>().enabled = false;
        gameOver.GetComponent<Canvas>().enabled = false;
        winMenu.GetComponent<Canvas>().enabled = false;
        Time.timeScale = 1f;
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            toggleMenu();
        }
    }

    public void toggleMenu()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            mainMenu.GetComponentInChildren<Canvas>().enabled = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Time.timeScale = 0f;
            mainMenu.GetComponentInChildren<Canvas>().enabled = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.SetCursor(cursor, new Vector2(cursor.width / 2, cursor.height / 2), CursorMode.Auto);
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        gameOver.GetComponentInChildren<Canvas>().enabled = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.SetCursor(cursor, new Vector2(cursor.width / 2, cursor.height / 2), CursorMode.Auto);
    }

    public void Win()
    {
        Time.timeScale = 0f;
        winMenu.GetComponentInChildren<Canvas>().enabled = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.SetCursor(cursor, new Vector2(cursor.width / 2, cursor.height / 2), CursorMode.Auto);
    }
}
