using UnityEngine;

public class GoalSensor : MonoBehaviour
{
    public GameManager gameManager;
    public int agentId;

    void OnTriggerEnter(Collider other)
    {
        //衝突したオブジェクトのレイヤーがballであれば
        if (other.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {
            gameManager.EndEpisode(agentId);
        }
    }
}
