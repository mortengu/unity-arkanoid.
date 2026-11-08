using UnityEngine;
using UnityEngine.InputSystem;

public class Ball : MonoBehaviour
{
    public Transform paddle;
    public float speed = 7f;

    Rigidbody2D body;
    bool launched;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        ResetBall();
    }

    void Update()
    {
        if (!launched)
        {
            // Wait just above the paddle until Space is pressed.
            transform.position = paddle.position + Vector3.up * 0.45f;

            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                launched = true;
                body.simulated = true;
                body.position = transform.position;
                float angle = Random.Range(-45f, 45f) * Mathf.Deg2Rad;
                body.linearVelocity = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * speed;
            }
        }
        // Once the ball is below the paddle, it has been missed.
        else if (body.position.y < paddle.position.y - 0.3f)
        {
            ResetBall();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Keep the ball at its chosen speed after each bounce.
        body.linearVelocity = body.linearVelocity.normalized * speed;
    }

    void ResetBall()
    {
        launched = false;
        body.linearVelocity = Vector2.zero;
        body.simulated = false;
        transform.position = paddle.position + Vector3.up * 0.45f;
    }
}
