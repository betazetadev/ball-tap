using UnityEngine;
using TMPro;

public class BallSpawn : MonoBehaviour, IBallTapHandler
{
    public GameObject ballPrefab;
    public TextMeshProUGUI tapCountText;

	private int totalResult = 0;

    void Start()
    {
		PlayerPrefs.DeleteKey("TouchCounter");
		PlayerPrefs.DeleteKey("TouchBonusCounter");
        // Spawn the ball object at the spawner's position and rotation
        GameObject ball = Instantiate(ballPrefab, transform.position, transform.rotation);
        ball.GetComponent<BallController>().SetTapHandler(this);
    }
    
    public void OnBallTapped()
    {
		int bonusPoints = PlayerPrefs.GetInt("TouchBonusCounter");
        totalResult += 1 + (bonusPoints / 2);
        tapCountText.text = "" + totalResult;
Debug.Log("+1 Tap + " + bonusPoints + " Bonus" + " = " + totalResult);
		PlayerPrefs.SetInt("TouchBonusCounter", 0);
    }
}