using UnityEngine;

public class Ball : MonoBehaviour
{
    private bool finished = false;

    void Update()
    {
        if (finished) return;

        // 
        if (transform.position.y < -5f)
        {
            finished = true;
            GameManager.Instance.OnBallLost(this);
            Destroy(gameObject);
        }
    }

    public void OnCaptured()
    {
        if (finished) return;

        finished = true;
        GameManager.Instance.OnBallCaptured(this);
    }
}
