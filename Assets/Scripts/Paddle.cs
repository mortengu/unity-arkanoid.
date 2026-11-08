using UnityEngine;
using UnityEngine.InputSystem;

public class Paddle : MonoBehaviour
{
    public float speed = 8f;
    public float horizontalLimit = 6f;

    Rigidbody2D body;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        float direction = 0f;
        if (keyboard.leftArrowKey.isPressed || keyboard.aKey.isPressed) direction -= 1f;
        if (keyboard.rightArrowKey.isPressed || keyboard.dKey.isPressed) direction += 1f;

        Vector2 position = body.position;
        position.x += direction * speed * Time.fixedDeltaTime;
        position.x = Mathf.Clamp(position.x, -horizontalLimit, horizontalLimit);
        body.MovePosition(position);
    }
}
