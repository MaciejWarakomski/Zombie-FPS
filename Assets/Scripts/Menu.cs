using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Texture GameLogo;
    public float buttonWidth = 300;
    public float buttonHeight = 60;
    public GUISkin skin;
    //public GUIStyle Button;

    private float buttonMargin = 20;


    // Start is called before the first frame update
    void Start()
    {
        buttonWidth = (buttonWidth * Screen.width) / 1920;
        buttonHeight = (buttonHeight * Screen.height) / 1080;
        buttonMargin = (buttonMargin * Screen.height) / 1080;
        skin.GetStyle("Button").fontSize = (int)(50 * Screen.height / 1080);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnGUI()
    {
        GUI.skin = skin;
        GUI.DrawTexture(new Rect(0, 0, 800 * Screen.width / 1920, 300 * Screen.height / 1080), GameLogo);

        GUI.BeginGroup(new Rect(300 * Screen.width / 1920, 300 * Screen.height / 1080, buttonWidth, (buttonHeight + buttonMargin) * 3));
        if (GUI.Button(new Rect(0, 0, buttonWidth, buttonHeight), "New Game"))
        {
            SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        }
        if (GUI.Button(new Rect(0, 0 + buttonHeight + buttonMargin, buttonWidth, buttonHeight), "Options"/*, Button*/))
        {

        }
        if (GUI.Button(new Rect(0, 0 + (buttonHeight + buttonMargin) * 2, buttonWidth, buttonHeight), "Exit"))
        {
            Application.Quit();
        }
        GUI.EndGroup();
    }
}
