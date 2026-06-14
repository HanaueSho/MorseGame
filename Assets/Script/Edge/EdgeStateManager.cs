using UnityEngine;

public class EdgeStateManager : MonoBehaviour
{
    // 経路か判定
    [Header("自身が経路かのフラグ")]
    [SerializeField] private bool _isRoute = false;

    // 流れる関係
    [Header("アニメーション処理関係")]
    [SerializeField] private bool _isFlow = true;
    [SerializeField] private float _flowSpeed = 1.0f;
    private LineRenderer _lineRenderer;
    private Material _material;
    private float _offsetX = 0.0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _material = new Material(_lineRenderer.material);
        _lineRenderer.material = _material;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isFlow)
        {
            _offsetX -= _flowSpeed * Time.deltaTime;
            _material.mainTextureOffset = new Vector2(_offsetX, 0.0f);
        }
    }

    // ルートに設定する関数
    public void SetRoute(bool b)
    {
        if (b)
        {
            _isRoute = true;
            _isFlow = true;
        }
        else
        {
            _isRoute = false;
            _isFlow = false;
        }

    }

    public void ResetEdgeAnimation()
    {
        _isRoute = false;
        _isFlow = false;

        _offsetX = 0.0f;
        _material.mainTextureOffset = new Vector2(_offsetX, 0.0f);
    }

    public bool IsRoute()
    {
        return _isRoute;
    }
}