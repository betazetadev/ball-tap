using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public Text touchCountText;

    void Start()
    {
        int touchCounter = PlayerPrefs.GetInt("TouchCounter");
        touchCountText.text = "" + touchCounter.ToString();
    }
}