using UnityEngine;
using TMPro;

public class BallSpawn : MonoBehaviour, IBallTapHandler
{
    public GameObject ballPrefab;
    public TextMeshProUGUI tapCountText;
	public TextMeshProUGUI topScoreText;

	private int totalResult = 0;

    void Start()
    {
		PlayerPrefs.DeleteKey("TouchCounter");
		PlayerPrefs.DeleteKey("TouchBonusCounter");
        // Spawn the ball object at the spawner's position and rotation
        GameObject ball = Instantiate(ballPrefab, transform.position, transform.rotation);
        ball.GetComponent<BallController>().SetTapHandler(this);
		DataService ds = new DataService("scores.db");
		Score topScore = ds.GetTopScore();
		topScoreText.text = "" + topScore.Value;
    }
    
    public void OnBallTapped()
    {
		int bonusPoints = PlayerPrefs.GetInt("TouchBonusCounter");
        totalResult += 1 + (bonusPoints / 2);
        tapCountText.text = "" + totalResult;
		PlayerPrefs.SetInt("TouchCounter", totalResult);
		PlayerPrefs.SetInt("TouchBonusCounter", 0);
    }
}