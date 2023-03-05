using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfScreenZone : MonoBehaviour
{
    public Color color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
    public float heightRatio = 0.5f;

    private void OnGUI()
    {
        GUI.color = color;

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        float rectHeight = screenHeight * heightRatio;
        Rect rect = new Rect(0, screenHeight - rectHeight, screenWidth, rectHeight);

        GUI.DrawTexture(rect, Texture2D.whiteTexture);
    }
}