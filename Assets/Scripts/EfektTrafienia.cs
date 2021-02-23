using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EfektTrafienia : MonoBehaviour
{
    public Texture2D bloodTexture;
    private bool hit = false;
    private float opacity = 0.0f;

    private void OnGUI()
    {
        if (hit)
        {
            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, opacity);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), bloodTexture, ScaleMode.ScaleToFit);
            StartCoroutine("waitAndChangeOpacity");
        }

        if (opacity <= 0)
        {
            hit = false;
        }
    }

    IEnumerator waitAndChangeOpacity()
    {
        yield return new WaitForEndOfFrame();
        opacity -= 0.01f;
    }

    void BloodEffect()
    {
        hit = true;
        opacity = 1.0f;
    }
}
