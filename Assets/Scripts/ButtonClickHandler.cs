using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClickHandler : MonoBehaviour
{

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
}