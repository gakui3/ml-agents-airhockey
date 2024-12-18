using UnityEngine;

public class BallSetting : MonoBehaviour
{
    public float maxSpeed = 10.0f; // ボールの最大速度
    public float minSpeed = 2.0f;  // ボールの最低速度

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 velocity = rb.linearVelocity;

        // 最大速度を制限
        if (velocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        }
        // 最低速度を確保
        else if (velocity.magnitude < minSpeed && velocity.magnitude > 0.01f)
        {
            rb.linearVelocity = velocity.normalized * minSpeed;
        }
    }
}
