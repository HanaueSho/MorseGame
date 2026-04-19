using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateManager : MonoBehaviour
{
    // ルートノード
    [SerializeField] private AlphabetStateManager _rootNode;

    // 現在のアルファベットノード
    [SerializeField] private AlphabetStateManager _alphabetNode;

    // 入力系
    private MobileTouchInput _touchInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _touchInput = GetComponent<MobileTouchInput>();
    }

    // Update is called once per frame
    void Update()
    {
        // タップ（トン）
        if (Keyboard.current.pKey.wasPressedThisFrame || _touchInput.OnTapReleased())
        {
            // Dit
            if (_alphabetNode != null)
            {
                if(_alphabetNode.DitNode != null)
                {
                    _alphabetNode = _alphabetNode.DitNode;
                    MoveNode(); // ノードへ移動
                }
                else
                {
                    _alphabetNode = _rootNode;
                    MoveRootNode();
                }
            }
        }
        // フリック（ツー）
        if (Keyboard.current.oKey.wasPressedThisFrame || _touchInput.OnFlickReleased())
        {
            // Dah
            if (_alphabetNode != null)
            {
                if (_alphabetNode.DahNode != null)
                {
                    _alphabetNode = _alphabetNode.DahNode;
                    MoveNode(); // ノードへ移動
                }
                else
                {
                    _alphabetNode = _rootNode;
                    MoveRootNode();
                }
            }
        }
    }

    private void MoveNode()
    {
        // 位置の更新
        Vector3 position = _alphabetNode.transform.position;
        transform.position = position;
    }

    private void MoveRootNode()
    {
        // 位置の更新
        Vector3 position = _rootNode.transform.position;
        transform.position = position;
    }

}
