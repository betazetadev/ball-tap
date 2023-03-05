using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public interface IBallTapHandler
{
    void OnBallTapped();
}

public class BallController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float horizontalForce = 5f;
    private Rigidbody2D rb;
    private Camera mainCamera;
    private Vector2 dragStartPosition;
    private Vector2 dragEndPosition;
    public TextMeshProUGUI touchCounterText;
    private IBallTapHandler tapHandler;
    private Vector3 defaultScale;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        defaultScale = transform.localScale;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Check if the click position is lower than half of the screen height
            float clickY = Input.mousePosition.y / Screen.height;
            if (clickY > 0.5f)
            {
                return;
            }
            
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hitCollider = Physics2D.OverlapPoint(mousePos);

            if (hitCollider != null && hitCollider.gameObject == gameObject)
            {
                // Calculate the horizontal force based on the distance between the click position and the ball center
                float distance = mousePos.x - transform.position.x;

                // Determine the horizontal direction based on the side of the touch on the ball
                float horizontalDirection = -(Mathf.Sign(distance) * -1f);

                // Increase the horizontal force multiplier based on the distance of the touch from the center of the ball
                float horizontalForceMultiplier = Mathf.Lerp(0.5f, 1f, horizontalForce);

                // Calculate the upward force to be applied to the ball
                float verticalForce = jumpForce * (1f + horizontalForce);

                // Apply the forces to the ball
                rb.velocity = Vector2.zero;
                float angle = Mathf.Lerp(10f, 70f, horizontalForce);
                Vector2 forceDirection = Quaternion.Euler(0f, 0f, angle * horizontalDirection) * Vector2.up;
                rb.AddForce(forceDirection * verticalForce, ForceMode2D.Impulse);

                StartCoroutine(SquashEffect());
                
                // Notify the tap handler that the ball was tapped
                if (tapHandler != null)
                {
                    tapHandler.OnBallTapped();
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            SceneManager.LoadScene("StartMenu");
        }
    }

    private void FixedUpdate()
    {
        if (transform.position.y < -5f)
        {
            transform.position = Vector2.zero;
            rb.velocity = Vector2.zero;
        }
    }

    public void SetTapHandler(IBallTapHandler tapHandler)
    {
        this.tapHandler = tapHandler;
    }
    
    IEnumerator SquashEffect() {
        transform.localScale = defaultScale * 0.8f;
        yield return new WaitForSeconds(0.1f);
        float duration = 0.1f;
        float time = 0f;
        while (time < duration) {
            transform.localScale = Vector3.Lerp(transform.localScale, defaultScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localScale = defaultScale;
    }
}