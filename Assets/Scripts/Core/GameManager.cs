using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int totalBalls = 5;
    private int finishedBalls = 0;

    void Awake()
    {
        Instance = this;
    }

    public void OnBallCaptured(Ball ball)
    {
        finishedBalls++;
        CheckEnd();
    }

    public void OnBallLost(Ball ball)
    {
        finishedBalls++;
        CheckEnd();
    }

    void CheckEnd()
    {
        if (finishedBalls >= totalBalls)
        {
            Debug.Log("Game Over");
            // TODO: UI / Restart
        }
    }
}
