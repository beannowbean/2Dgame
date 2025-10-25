using UnityEngine;

public class EagleController : MonoBehaviour
{
    public float moveSpeed = 3.0f; // Speed of vertical movement
    public float topLimitY = 5.0f;   // Y coordinate of the ceiling
    public float bottomLimitY = -5.0f; // Y coordinate of the floor

    // Component reference
    private Rigidbody2D rb;

    // State variable
    private bool movingUp = true; // Is the eagle currently moving upwards?

    void Start()
    {
        // 1. Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();

        // 2. Initial setup (optional: turn off gravity, freeze rotation)
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; // Only allow Y movement

        // 3. Set initial velocity based on starting direction
        rb.linearVelocity = new Vector2(0, moveSpeed);
    }

    void Update()
    {
        // 1. Check if the eagle has reached or exceeded the top limit
        if (movingUp && transform.position.y >= topLimitY)
        {
            // Reverse direction to move down
            movingUp = false;
            rb.linearVelocity = new Vector2(0, -moveSpeed);
        }
        // 2. Check if the eagle has reached or fallen below the bottom limit
        else if (!movingUp && transform.position.y <= bottomLimitY)
        {
            // Reverse direction to move up
            movingUp = true;
            rb.linearVelocity = new Vector2(0, moveSpeed);
        }
    }
}