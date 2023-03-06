using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextFadeAnimation : MonoBehaviour
{
    public Text textObject;
    public float fadeInTime = 2f;
    public float displayTime = 5f;
    public float fadeOutTime = 2f;

    private Color startColor;

    private void Start()
    {
        // Disable the text object
        textObject.enabled = false;

        // Set the start color
        startColor = textObject.color;

        // Start the coroutine to display the text
        StartCoroutine(EnterText());
    }

    private IEnumerator EnterText()
    {
        // Enable the text object
        textObject.enabled = true;

        // Gradually increase the alpha over time
        for (float t = 0; t < fadeInTime; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeInTime;
            textObject.color = new Color(startColor.r, startColor.g, startColor.b, normalizedTime);
            yield return null;
        }

        textObject.color = new Color(startColor.r, startColor.g, startColor.b, 1f);

        // Display the text for a set amount of time
        yield return new WaitForSeconds(displayTime);

        // Gradually decrease the alpha over time
        for (float t = 0; t < fadeOutTime; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeOutTime;
            textObject.color = new Color(startColor.r, startColor.g, startColor.b, 1 - normalizedTime);
            yield return null;
        }

        textObject.color = new Color(startColor.r, startColor.g, startColor.b, 0f);

        // Disable the text object
        textObject.enabled = false;
    }
}