using UnityEngine;

public class BallSpawn : MonoBehaviour
{
    public GameObject ballPrefab;

    void Start()
    {
        // Spawn the ball object at the spawner's position and rotation
        GameObject ball = Instantiate(ballPrefab, transform.position, transform.rotation);
    }
}