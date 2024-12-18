using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class PaddleAgentWithContinuous : Agent
{
    public int agentId; // エージェントのID (0または1)
    public GameObject ball; // ボールオブジェクト
    public GameObject enemyPaddle; // 敵パドルオブジェクト
    private Rigidbody ballRb; // ボールのRigidbody
    private Rigidbody rb; // パドルのRigidbody

    private float playerSpeed = 1.0f; // パドルの速度

    public override void Initialize()
    {
        ballRb = ball.GetComponent<Rigidbody>();
        rb = GetComponent<Rigidbody>();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        float dir = agentId == 0 ? 1.0f : -1.0f;

        // ボールの位置と速度（自分視点で統一）
        sensor.AddObservation(ball.transform.localPosition.x * dir);
        sensor.AddObservation(ball.transform.localPosition.z * dir);
        sensor.AddObservation(ballRb.linearVelocity.x * dir);
        sensor.AddObservation(ballRb.linearVelocity.z * dir);

        // 自分のパドルの位置（自分視点で統一）
        sensor.AddObservation(transform.localPosition.x * dir);
        sensor.AddObservation(transform.localPosition.z * dir);
        sensor.AddObservation(rb.linearVelocity.x * dir);
        sensor.AddObservation(rb.linearVelocity.z * dir);

        // 敵パドルの位置（自分視点で統一）
        sensor.AddObservation(enemyPaddle.transform.localPosition.x * dir);
        sensor.AddObservation(enemyPaddle.transform.localPosition.z * dir);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {
            // ボールとの衝突時に報酬を追加
            AddReward(0.0025f);
        }
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        float dir = agentId == 0 ? 1.0f : -1.0f;

        // ContinuousActionsから移動量を取得
        float velZ = actionBuffers.ContinuousActions[0] * dir; // x方向の速度
        float velX = actionBuffers.ContinuousActions[1] * dir; // z方向の速度

        //現在のrbの速度を取得
        Vector3 currentVelocity = rb.linearVelocity;

        //新しい速度を計算
        Vector3 newVelocity = currentVelocity + new Vector3(velX * playerSpeed, 0, velZ * playerSpeed);

        //位置を更新
        Vector3 targetPosition = transform.localPosition + newVelocity * Time.deltaTime;

        if (agentId == 0)
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, 0.65f, 2.65f);
        }
        else
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, -2.65f, -0.65f);
        }
        targetPosition.z = Mathf.Clamp(targetPosition.z, -1.4f, 1.4f);

        newVelocity.x = (targetPosition.x - transform.localPosition.x) / Time.deltaTime;
        newVelocity.z = (targetPosition.z - transform.localPosition.z) / Time.deltaTime;

        // Rigidbodyの速度を設定
        rb.linearVelocity = newVelocity;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {

    }
}
