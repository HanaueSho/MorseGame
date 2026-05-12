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
            if (_alphabetNode != null)
            {
                if(_alphabetNode.DitNode != null)
                {
                    _alphabetNode = _alphabetNode.DitNode;
                    MoveNode(); // ノードへ移動
                }
                else
                {
                    MoveRootNode(); // ルートへ移動
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
                    MoveRootNode(); // ルートへ移動
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
