using UnityEngine;

/// <summary>
/// このオブジェクトの RectTransform を、端末の safe area に合わせるスクリプト。
///
/// 使い方:
/// - Canvas 配下に「SafeAreaRoot」のような空の UI オブジェクトを作る
/// - その RectTransform にこのスクリプトを付ける
/// - 重要なUI（スコア、ボタン、HPバーなど）をその子に入れる
///
/// すると、ノッチ・角丸・ホームインジケータにかかりにくくなる。
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class SafeAreaFitter : MonoBehaviour
{
    // 自分自身の RectTransform を保持するのだ
    private RectTransform _rectTransform;

    // 前回適用した safe area を覚えておくのだ
    private Rect _lastSafeArea = new Rect(0, 0, 0, 0);

    // 前回の画面サイズを覚えておくのだ
    private Vector2Int _lastScreenSize = Vector2Int.zero;

    // 前回の画面向きを覚えておくのだ
    private ScreenOrientation _lastOrientation = ScreenOrientation.AutoRotation;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        ApplySafeArea();
    }

    private void Update()
    {
        // 画面回転や端末状態の変化で safe area が変わることがあるので、
        // 前回と違っていたら再適用するのだ
        if (_lastSafeArea != Screen.safeArea ||
            _lastScreenSize.x != Screen.width ||
            _lastScreenSize.y != Screen.height ||
            _lastOrientation != Screen.orientation)
        {
            ApplySafeArea();
        }
    }

    /// <summary>
    /// 現在の Screen.safeArea をもとに、
    /// RectTransform の anchorMin / anchorMax を設定するのだ
    /// </summary>
    private void ApplySafeArea()
    {
        Rect safeArea = Screen.safeArea;

        // 現在値を保存して、次回比較に使うのだ
        _lastSafeArea = safeArea;
        _lastScreenSize = new Vector2Int(Screen.width, Screen.height);
        _lastOrientation = Screen.orientation;

        // Screen.safeArea はピクセル座標で返るのだ
        // RectTransform の anchor は 0～1 の正規化座標なので、
        // 画面サイズで割って変換するのだ
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        // 親UIを safe area の範囲に合わせるのだ
        _rectTransform.anchorMin = anchorMin;
        _rectTransform.anchorMax = anchorMax;

        // anchor にぴったり追従するように余計な offset を消すのだ
        _rectTransform.offsetMin = Vector2.zero;
        _rectTransform.offsetMax = Vector2.zero;
    }
}