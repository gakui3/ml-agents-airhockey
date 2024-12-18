using UnityEngine;

public class TimeScaleManager : MonoBehaviour
{
    [Range(0.1f, 20f)]
    public float timeScale = 1.0f; // タイムスケールの初期値

    private void Start()
    {
        timeScale = Time.timeScale;
        // Time.timeScale = timeScale;
    }

    private void OnValidate()
    {
        // インスペクターで値を変更した際にTime.timeScaleを更新
        Time.timeScale = timeScale;

        // Debug.Logを使って変更を確認（デバッグ用、必要に応じて削除）
        // Debug.Log($"Time.timeScale updated to: {Time.timeScale}");
    }
}
