using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ButtonClickHandler : MonoBehaviour
{

	public GameObject comboText; 

    // This method will be called when the button is clicked
    public void MainGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void StartMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
    
    public void EndGame()
    {
        // Add code here to perform any actions before quitting the game

        // Quit the game
        Application.Quit();
    }

    public void CircleGestureDetected()
    {
    Debug.Log("Circle gesture detected");
    comboText.SetActive(true); // Show the text object
	comboText.GetComponent<TextMeshProUGUI>().text = "+ " + (PlayerPrefs.GetInt("TouchBonusCounter") / 2);	
    Invoke("HideText", 2f); // Schedule the HideText method to be called after 2 seconds
    }

private void HideText()
{
    comboText.SetActive(false); // Hide the text object
}
}