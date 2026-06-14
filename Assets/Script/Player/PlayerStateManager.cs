using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateManager : MonoBehaviour
{
    // ルートノード
    [Header("ルートノード参照")]
    [SerializeField] private AlphabetStateManager _rootNode;

    // 現在のアルファベットノード
    [Header("現在位置のアルファベットノード")]
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
            RequestMoveNode(0);
        }
        // フリック（ツー）
        if (Keyboard.current.oKey.wasPressedThisFrame || _touchInput.OnFlickReleased())
        {
            // Dah
            RequestMoveNode(1);
        }
    }

    // 移動のリクエスト
    private void RequestMoveNode(int ditdah)
    {
        if (_alphabetNode == null) return;

        if (ditdah == 0) // タップ（トン）
        {
            if (_alphabetNode.DitNode != null)
            {
                if (_alphabetNode.DitNode.IsRightNode())
                {
                    MoveNode(_alphabetNode.DitNode); // ノードへ移動
                    return;
                }
            }
        }
        else if (ditdah == 1) // フリック（ツー）
        {
            if (_alphabetNode.DahNode != null)
            {
                if (_alphabetNode.DahNode.IsRightNode())
                {
                    MoveNode(_alphabetNode.DahNode); // ノードへ移動
                    return;
                }
            }
        }


        MoveRootNode(); // ルートへ移動
    }

    private void MoveNode(AlphabetStateManager alphabetNode)
    {
        // ノード更新
        _alphabetNode = alphabetNode;

        // 位置の更新
        Vector3 position = _alphabetNode.transform.position;
        transform.position = position;
    }

    public void MoveRootNode()
    {
        // ルートノードへ移動
        _alphabetNode = _rootNode;

        // 位置の更新
        Vector3 position = _rootNode.transform.position;
        transform.position = position;
    }


    // 正解のノードへたどり着いたか判定
    public bool IsGetAnswerNode()
    {
        if (!_alphabetNode._isAnswerNode)
            return false;

        // 色を戻してあげる
        _alphabetNode.OffLight();

        // エッジアニメーションのリセット
        _alphabetNode.StopAnimationEdge();


        // ルートノードへ戻す
        MoveRootNode();

        return true;
    }


    // QuestionManager から呼ばれる
    public void PrepareQuestionAlphabet(int[] array)
    {
        // １文字目のトンツーを呼ぶ
        if (array[0] == 1) // トン
        {
            _rootNode.DitNode.Dit(array, 0);
            _rootNode.DitEdge.SetRoute(true);
        }
        else if (array[0] == 2) // ツー
        {
            _rootNode.DahNode.Dah(array, 0);
            _rootNode.DahEdge.SetRoute(true);
        }
    }
}
