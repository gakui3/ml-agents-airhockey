using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public float xLimitMin = 0.65f;   // x方向の移動範囲制限（最小値）
    public float xLimitMax = 2.65f;   // x方向の移動範囲制限（最大値）
    public float zLimit = 1.4f;       // z方向の移動範囲制限

    private Camera mainCamera;        // メインカメラの参照
    private Vector3 offset;           // マウスの位置とオブジェクトのオフセット
    private Vector3 initialPosition;  // 初期位置の記録
    private Rigidbody rb;             // Rigidbodyの参照

    private void Start()
    {
        // メインカメラを取得
        mainCamera = Camera.main;

        // Rigidbodyを取得
        rb = GetComponent<Rigidbody>();

        // 初期位置を記録
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 左クリックが押された瞬間
        {
            // マウス位置からローカル座標を取得し、オフセットを計算
            Vector3 mouseLocalPosition = GetMouseLocalPosition();
            offset = transform.localPosition - mouseLocalPosition;
        }

        if (Input.GetMouseButton(0)) // 左クリック押下中
        {
            // マウス位置をローカル座標で計算し、オフセットを考慮
            Vector3 targetLocalPosition = GetMouseLocalPosition() + offset;

            // 範囲制限を適用
            targetLocalPosition.x = Mathf.Clamp(targetLocalPosition.x, xLimitMin, xLimitMax);
            targetLocalPosition.z = Mathf.Clamp(targetLocalPosition.z, -zLimit, zLimit);
            targetLocalPosition.y = initialPosition.y; // Y軸を固定

            // ローカル座標をワールド座標に変換
            Vector3 targetWorldPosition = transform.parent.TransformPoint(targetLocalPosition);

            // RigidbodyのMovePositionを使用して位置を更新
            rb.MovePosition(targetWorldPosition);
        }
    }

    // マウスのスクリーン座標をローカル座標に変換する関数
    private Vector3 GetMouseLocalPosition()
    {
        // マウスのスクリーン座標をワールド座標に変換
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = Mathf.Abs(mainCamera.transform.position.y - transform.position.y); // 奥行き(Z軸)を指定

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);

        // ワールド座標を親オブジェクトのローカル座標に変換
        return transform.parent.InverseTransformPoint(worldPosition);
    }
}
