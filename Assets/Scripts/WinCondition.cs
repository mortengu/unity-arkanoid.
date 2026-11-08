using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    public GameObject winMessage;
    public Ball ball;
    public Paddle paddle;

    bool won;

    void Update()
    {
        if (!won && transform.childCount == 0)
        {
            won = true;
            winMessage.SetActive(true);
            ball.gameObject.SetActive(false);
            paddle.enabled = false;
        }

        // After winning, R reloads the scene for a fresh game.
        if (won && Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
