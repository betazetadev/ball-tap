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
                
                Vector2 touchPosition = Input.mousePosition;
         //       RotateBall(touchPosition);
                StartCoroutine(SquashEffect(touchPosition));
                
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
    
    IEnumerator SquashEffect(Vector2 touchPosition)
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = startScale;

        // Calculate how much to squash the ball
        float maxSquash = 0.2f;
        float minSquash = 0.02f;
        float distanceFromCenter = Mathf.Abs(touchPosition.y - Screen.height / 2f);
        float squashFactor = Mathf.Clamp(distanceFromCenter / (Screen.height / 2f), 0f, 1f);
        float squash = Mathf.Lerp(maxSquash, minSquash, squashFactor);

        // Squash in the touch area
        endScale.x += squash;
        endScale.y -= squash;

        // Squash in the opposite direction of the touch
        if (touchPosition.y > Screen.height / 2f)
        {
            endScale.x -= squash;
            endScale.y += squash;
        }

        // Animate the squash effect
        float duration = 0.2f;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset the scale to the original size
        transform.localScale = startScale;
    }
    
    /*void RotateBall(Vector2 touchPosition)
    {
        // Calculate the direction from the ball to the touch position
        Vector2 direction = (touchPosition - (Vector2)transform.position).normalized;

        // Calculate the angle between the direction and the up vector
        float angle = Vector2.SignedAngle(Vector2.up, direction);

        // Rotate the ball by that angle
        transform.Rotate(0, 0, angle);
    }*/
}