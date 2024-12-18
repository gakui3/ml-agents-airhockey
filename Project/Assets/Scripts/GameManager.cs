using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;

public class GameManager : MonoBehaviour
{

    public Agent[] agents;
    public GameObject ball;

    public bool isTraining = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Reset();
        StartCoroutine(ResetCoroutine());
    }

    IEnumerator ResetCoroutine()
    {
        while (true) // 無限ループ
        {
            //現在のballのx座標を保存
            float x = ball.transform.localPosition.x;
            // 現在のballの速度を保存
            Vector3 velocity = ball.GetComponent<Rigidbody>().linearVelocity;
            yield return new WaitForSeconds(5.0f);

            //もしballのx座標がほぼ変わっていなかったらreset
            // float diff = Mathf.Abs(ball.transform.localPosition.x - x);
            // if (diff < 0.2f)
            // {
            //     Debug.Log("x座標がほぼ変わっていないのでreset");
            //     Reset();
            // }

            //もしボールのx座標,z座標がおかしなところに行っていたらreset
            if (Mathf.Abs(ball.transform.localPosition.x) > 3.5f || Mathf.Abs(ball.transform.localPosition.z) > 3.5f)
            {
                // Debug.Log("ボールの座標がおかしいのでreset");
                Reset();
            }

            //もし現在のボールの速度と保存した速度のx,z成分がほぼ同じだったらreset
            Vector3 currentVelocity = ball.GetComponent<Rigidbody>().linearVelocity;
            if (Mathf.Abs(velocity.x - currentVelocity.x) < 0.025f || Mathf.Abs(velocity.z - currentVelocity.z) < 0.025f)
            {
                Debug.Log("ボールの速度がほぼ変わっていないのでreset");
                Reset();
            }

        }
    }


    public void Reset()
    {
        float speed = 5.0f;
        agents[0].gameObject.transform.localPosition = new Vector3(2.65f, 0.2f, 0);
        agents[1].gameObject.transform.localPosition = new Vector3(-2.65f, 0.2f, 0);

        //agentsのrbの速度も初期化
        // agents[0].GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        // agents[1].GetComponent<Rigidbody>().linearVelocity = Vector3.zero;

        ball.transform.localPosition = new Vector3(0, 0.2f, 0);
        ball.transform.rotation = Quaternion.identity;
        float radius = Random.Range(140f, 220f) * Mathf.PI / 180.0f;
        Vector3 force = new Vector3(Mathf.Cos(radius) * speed, 0.0f, Mathf.Sin(radius) * speed);
        if (Random.value < 0.5f) force.z = -force.z;
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.linearVelocity = force;
    }

    public void EndEpisode(int agentId)
    {
        if (agentId == 0)
        {
            agents[1].AddReward(1.0f);
            agents[0].AddReward(-1.0f);
        }
        else
        {
            agents[0].AddReward(1.0f);
            agents[1].AddReward(-1.0f);
        }
        agents[0].EndEpisode();
        agents[1].EndEpisode();
        Reset();
    }

    public void SetPlayerVsCPU()
    {
        Rigidbody playerRigidbody = agents[0].GetComponent<Rigidbody>();
        PaddleController paddleController = agents[0].GetComponent<PaddleController>();
        // PlayerのRigidbodyをKinematicにする
        playerRigidbody.isKinematic = true;

        // PaddleControllerを有効化
        if (paddleController != null)
        {
            paddleController.enabled = true;
        }

        //canvasを非表示にする
        GameObject.Find("Canvas").SetActive(false);

        Debug.Log("Player vs CPU モードが設定されました。");
    }

    // CPU vs CPU モード
    public void SetCPUvsCPU()
    {
        Rigidbody playerRigidbody = agents[0].GetComponent<Rigidbody>();
        PaddleController paddleController = agents[0].GetComponent<PaddleController>();

        // PlayerのRigidbodyをKinematicを解除
        playerRigidbody.isKinematic = false;

        // PaddleControllerを無効化
        if (paddleController != null)
        {
            paddleController.enabled = false;
        }

        //canvasを非表示にする
        GameObject.Find("Canvas").SetActive(false);

        Debug.Log("CPU vs CPU モードが設定されました。");
    }
}

