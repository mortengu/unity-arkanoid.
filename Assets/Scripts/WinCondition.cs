using UnityEngine;

public class WinCondition : MonoBehaviour
{
    public GameObject winMessage;
    public Ball ball;
    public Paddle paddle;

    void Update()
    {
        if (transform.childCount == 0)
        {
            winMessage.SetActive(true);
            ball.gameObject.SetActive(false);
            paddle.enabled = false;
            enabled = false;
        }
    }
}
