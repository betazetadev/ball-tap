using UnityEngine;
using TMPro;

public class BallSpawn : MonoBehaviour, IBallTapHandler
{
    public GameObject ballPrefab;
    public TextMeshProUGUI tapCountText;
    private int tapCount = 0;

    void Start()
    {
		PlayerPrefs.DeleteKey("TouchCounter");
        // Spawn the ball object at the spawner's position and rotation
        GameObject ball = Instantiate(ballPrefab, transform.position, transform.rotation);
        ball.GetComponent<BallController>().SetTapHandler(this);
        tapCount = 0;
    }
    
    public void OnBallTapped()
    {
        tapCount++;
        tapCountText.text = "" + tapCount.ToString();
		PlayerPrefs.SetInt("TouchCounter", tapCount);
    }
}