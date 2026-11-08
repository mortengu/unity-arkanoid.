using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Lives : MonoBehaviour
{
    public int lives = 3;
    public TMP_Text livesText;
    public GameObject gameOverMessage;
    public Ball ball;
    public Paddle paddle;

    bool gameOver;

    void Start()
    {
        livesText.text = "Lives: " + lives;
    }

    // Called by the ball each time it falls past the paddle.
    public void LoseLife()
    {
        lives--;
        livesText.text = "Lives: " + lives;

        if (lives <= 0)
        {
            gameOver = true;
            gameOverMessage.SetActive(true);
            ball.gameObject.SetActive(false);
            paddle.enabled = false;
        }
    }

    void Update()
    {
        // After game over, R reloads the scene for a fresh game.
        if (gameOver && Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
