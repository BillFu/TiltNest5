using UnityEngine;

public class Hole : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ball")) return;

        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb == null) return;

        // 
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // 
        rb.isKinematic = true;

        // 
        other.transform.position = transform.position;

        // 
        Ball ball = other.GetComponent<Ball>();
        if (ball != null) ball.OnCaptured();
    }
}
