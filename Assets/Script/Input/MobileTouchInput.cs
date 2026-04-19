using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class MobileTouchInput : MonoBehaviour
{
    // タップ判定に使う設定値
    [SerializeField] private float _tapMaxTime = 0.2f;
    [SerializeField] private float _tapMaxDistance = 20f;
    [SerializeField] private float _flickMinDistance = 80f;

    // 最初に触れた1本だけを追跡するためのID
    // -1 は未追跡
    private int _trackingFingerId = -1;

    // タッチ開始情報
    private Vector2 _startPosition;
    private float _startTime;

    // このフレームで確定した入力結果
    private bool _tapReleasedThisFrame;
    private bool _flickReleasedThisFrame;

    // フリック結果を外から参照したいとき用
    private Vector2 _flickDirection;
    private Vector2 _endPosition;

    // 外部公開用プロパティ
    public Vector2 FlickDirection => _flickDirection;
    public Vector2 EndPosition => _endPosition;

    private void OnEnable()
    {
        // EnhancedTouch を使うために有効化
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    private void Update()
    {
        // 毎フレーム最初にリセット
        _tapReleasedThisFrame = false;
        _flickReleasedThisFrame = false;

        foreach (var touch in Touch.activeTouches)
        {
            // まだ追跡中の指がいない場合だけ、最初の1本を採用する
            if (_trackingFingerId == -1 && touch.phase == TouchPhase.Began)
            {
                _trackingFingerId = touch.touchId;
                _startPosition = touch.screenPosition;
                _startTime = Time.time;
                continue;
            }

            // 追跡している指以外は無視
            if (touch.touchId != _trackingFingerId)
            {
                continue;
            }

            // 追跡中の指が離れた瞬間に確定判定
            if (touch.phase == TouchPhase.Ended)
            {
                _endPosition = touch.screenPosition;

                float touchTime = Time.time - _startTime;
                float distance = Vector2.Distance(_startPosition, _endPosition);

                // 短時間かつほとんど動いていなければタップ
                if (touchTime <= _tapMaxTime && distance <= _tapMaxDistance)
                {
                    _tapReleasedThisFrame = true;
                }
                // 一定以上動いていればフリック
                else if (distance >= _flickMinDistance)
                {
                    _flickReleasedThisFrame = true;
                    _flickDirection = (_endPosition - _startPosition).normalized;
                }

                // 追跡終了
                _trackingFingerId = -1;
            }

            // キャンセルされた場合も追跡終了
            if (touch.phase == TouchPhase.Canceled)
            {
                _trackingFingerId = -1;
            }
        }
    }

    // このフレームでタップが確定したか
    public bool OnTapReleased()
    {
        return _tapReleasedThisFrame;
    }

    // このフレームでフリックが確定したか
    public bool OnFlickReleased()
    {
        return _flickReleasedThisFrame;
    }

    // フリック方向を文字列で欲しい場合
    public string GetFlickDirectionName()
    {
        if (!_flickReleasedThisFrame)
        {
            return "";
        }

        if (Mathf.Abs(_flickDirection.x) > Mathf.Abs(_flickDirection.y))
        {
            return _flickDirection.x > 0 ? "Right" : "Left";
        }
        else
        {
            return _flickDirection.y > 0 ? "Up" : "Down";
        }
    }
}