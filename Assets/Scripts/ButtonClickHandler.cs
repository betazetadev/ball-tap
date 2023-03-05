using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClickHandler : MonoBehaviour
{

    // This method will be called when the button is clicked
    public void OnButtonClick()
    {
        SceneManager.LoadScene("Main");
    }
    
    public void EndGame()
    {
        // Add code here to perform any actions before quitting the game

        // Quit the game
        Application.Quit();
    }
}