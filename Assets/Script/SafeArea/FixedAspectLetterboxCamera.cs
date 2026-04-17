using UnityEngine;

/// <summary>
/// 基準アスペクト比に対して、端末の「短い辺」を優先して合わせるのだ。
/// 余った長い辺には黒帯が出るのだ。
///
/// 例:
/// 基準が 9:16 のとき
/// - 画面が 9:20 のように縦長なら、横をぴったり合わせて上下に黒帯
/// - 画面が 20:16 のように横長なら、縦をぴったり合わせて左右に黒帯
/// </summary>
[RequireComponent(typeof(Camera))]
public class FitShortSideLetterboxCamera : MonoBehaviour
{
    [Header("基準アスペクト比 (幅 / 高さ)")]
    [SerializeField] private float _targetAspect = 9f / 16f;

    private Camera _camera;
    private int _lastScreenWidth;
    private int _lastScreenHeight;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        ApplyViewport();
    }

    private void Update()
    {
        if (_lastScreenWidth != Screen.width || _lastScreenHeight != Screen.height)
        {
            ApplyViewport();
        }
    }

    private void ApplyViewport()
    {
        _lastScreenWidth = Screen.width;
        _lastScreenHeight = Screen.height;

        float screenAspect = (float)Screen.width / Screen.height;

        Rect rect = new Rect(0f, 0f, 1f, 1f);

        if (screenAspect < _targetAspect)
        {
            // 端末のほうが基準より縦長（細い）のだ
            // 横をぴったり使い、高さを縮めるので上下に黒帯が出るのだ
            float normalizedHeight = screenAspect / _targetAspect;

            rect.width = 1f;
            rect.height = normalizedHeight;
            rect.x = 0f;
            rect.y = (1f - rect.height) * 0.5f;
        }
        else if (screenAspect > _targetAspect)
        {
            // 端末のほうが基準より横長（広い）のだ
            // 縦をぴったり使い、幅を縮めるので左右に黒帯が出るのだ
            float normalizedWidth = _targetAspect / screenAspect;

            rect.width = normalizedWidth;
            rect.height = 1f;
            rect.x = (1f - rect.width) * 0.5f;
            rect.y = 0f;
        }
        else
        {
            // 基準比率と同じなら黒帯なしなのだ
            rect = new Rect(0f, 0f, 1f, 1f);
        }

        _camera.rect = rect;
    }
}