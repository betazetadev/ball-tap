using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float horizontalForce = 5f;
    private Rigidbody2D rb;
    private Camera mainCamera;
    private Vector2 dragStartPosition;
    private Vector2 dragEndPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hitCollider = Physics2D.OverlapPoint(mousePos);

            if (hitCollider != null && hitCollider.gameObject == gameObject)
            {
                // Calculate the horizontal force based on the distance between the click position and the ball center
                float distance = mousePos.x - transform.position.x;

                // Determine the horizontal direction based on the side of the touch on the ball
                float horizontalDirection = Mathf.Sign(distance) * -1f;

                // Increase the horizontal force multiplier based on the distance of the touch from the center of the ball
                float horizontalForceMultiplier = Mathf.Lerp(0.5f, 1f, horizontalForce);

                // Calculate the upward force to be applied to the ball
                float verticalForce = jumpForce * (1f + horizontalForce);

                // Apply the forces to the ball
                rb.velocity = Vector2.zero;
                float angle = Mathf.Lerp(10f, 70f, horizontalForce);
                Vector2 forceDirection = Quaternion.Euler(0f, 0f, angle * horizontalDirection) * Vector2.up;
                rb.AddForce(forceDirection * verticalForce, ForceMode2D.Impulse);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Game Over");
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

    private void OnMouseDown()
    {
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hitCollider = Physics2D.OverlapPoint(mousePos);

        if (hitCollider != null && hitCollider.gameObject == gameObject)
        {
            // Calculate the horizontal force based on the distance between the click position and the ball center
            float distance = mousePos.x - transform.position.x;
            float horizontalRatio = Mathf.Abs(distance) / (transform.localScale.x / 2f);

            // Determine the horizontal direction based on the side of the touch on the ball
            float horizontalDirection = Mathf.Sign(distance) * -1f;

            // Increase the horizontal force multiplier based on the distance of the touch from the center of the ball
            float horizontalForceMultiplier = Mathf.Lerp(0.5f, 1f, horizontalRatio);

            // Calculate the vertical force based on the speed of the mouse drag
            float verticalSpeed = Mathf.Clamp(rb.velocity.y, -5f, 5f);
            float verticalForce = jumpForce * verticalSpeed / 5f;

            // Apply the forces to the ball
            rb.velocity = Vector2.zero;
            float angle = Mathf.Lerp(10f, 70f, horizontalRatio);
            Vector2 forceDirection = Quaternion.Euler(0f, 0f, angle * horizontalDirection) * Vector2.up;
            rb.AddForce(forceDirection * jumpForce * horizontalForceMultiplier, ForceMode2D.Impulse);
            rb.AddForce(Vector2.up * verticalForce, ForceMode2D.Impulse);
        }
    }
}