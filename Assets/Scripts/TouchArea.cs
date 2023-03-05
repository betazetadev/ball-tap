using UnityEngine;

public class TouchArea : MonoBehaviour
{
    public GameObject ball;
    public float forceMultiplier = 100f;
    public float maxHorizontalForce = 10f;
    public float touchRadius = 1f;
    private Vector3 touchPos;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Use a 2D raycast to detect a touch on the square
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                // Get the touch position in world space
                touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                touchPos.z = ball.transform.position.z;

                // Calculate the force to apply to the ball
                Vector3 force = Vector3.up * forceMultiplier;
                float distanceFromCenter = Mathf.Abs(touchPos.x - ball.transform.position.x);
                float horizontalForce = Mathf.Clamp(distanceFromCenter, 0f, maxHorizontalForce);
                force.x = Mathf.Sign(touchPos.x - ball.transform.position.x) * horizontalForce;

                // Apply the force to the ball
                ball.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, touchRadius);
    }
}