using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System;

public class PaddleAgentWithDiscrete : Agent
{
    public int agentId;
    public GameObject ball;
    Rigidbody ballRb;

    float playerSpeed = 0.075f;

    public override void Initialize()
    {
        ballRb = ball.GetComponent<Rigidbody>();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        float dir = agentId == 0 ? 1.0f : -1.0f;

        sensor.AddObservation(ball.transform.localPosition.x * dir);
        sensor.AddObservation(ball.transform.localPosition.z * dir);
        sensor.AddObservation(ballRb.linearVelocity.x * dir);
        sensor.AddObservation(ballRb.linearVelocity.z * dir);
        sensor.AddObservation(transform.localPosition.z * dir);
    }

    void OnCollisionEnter(Collision collision)
    {
        //もし衝突したもののレイヤーが"ball"だった場合、速度を1.05倍にする
        if (collision.gameObject.layer == LayerMask.NameToLayer("ball"))
        {
            ballRb.linearVelocity *= 1.01f;
        }
        AddReward(0.01f);
    }

    override public void OnActionReceived(ActionBuffers actionBuffers)
    {
        float dir = agentId == 0 ? 1.0f : -1.0f;

        // 離散アクションを取得
        int action = actionBuffers.DiscreteActions[0];

        Vector3 pos = transform.localPosition;

        // アクションの値によって動作を分岐
        if (action == 1) // 左に動く
        {
            pos.z -= playerSpeed * dir;
        }
        else if (action == 2) // 右に動く
        {
            pos.z += playerSpeed * dir;
        }
        // 何もしない (action == 0) は特に操作を加えない

        // 範囲制限
        if (pos.z < -1.4f) pos.z = -1.4f;
        if (pos.z > 1.4f) pos.z = 1.4f;

        transform.localPosition = pos;
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = 0; // 静止（デフォルト）

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            discreteActionsOut[0] = 1; // 左に動く
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            discreteActionsOut[0] = 2; // 右に動く
        }
    }

}
