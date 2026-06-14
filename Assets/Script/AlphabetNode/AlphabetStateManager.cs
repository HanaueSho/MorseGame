using UnityEngine;

public class AlphabetStateManager : MonoBehaviour
{
    // 自分のアルファベット
    [Header("自身のアルファベット設定")]
    [SerializeField] private string _myAlphabet = "A";
    public string MyAlphabet => _myAlphabet;

    // 隣り合うアルファベットの参照
    [Header("次のアルファベット参照")]
    [SerializeField] private AlphabetStateManager _ditNode; // トン
    [SerializeField] private AlphabetStateManager _dahNode; // ツー
    public AlphabetStateManager DitNode => _ditNode;
    public AlphabetStateManager DahNode => _dahNode;

    // 隣り合うアルファベットへのエッジの参照
    [Header("次のノードを繋ぐエッジ参照")]
    [SerializeField] private EdgeStateManager _ditEdge; // トン
    [SerializeField] private EdgeStateManager _dahEdge; // ツー
    public EdgeStateManager DitEdge => _ditEdge;
    public EdgeStateManager DahEdge => _dahEdge;

    // ひとつ前のアルファベットの参照
    [Header("前のアルファベット参照")]
    [SerializeField] private AlphabetStateManager _prevNode; // トン
    public AlphabetStateManager PrevNode => _prevNode;

    // ひとつ前のアルファベットへのエッジの参照
    [Header("前のノードを繋ぐエッジ参照")]
    [SerializeField] private EdgeStateManager _prevEdge; // トン
    public EdgeStateManager PrevEdge => _prevEdge;


    // 目的のノードかフラグ
    [Header("自身がゴールのノードかのフラグ")]
    public bool _isAnswerNode = false;


    // OnValidate は、Unityエディタ上で値が変更されたときに呼ばれるメソッド
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(_myAlphabet))
        {
            _myAlphabet = "A";
            return;
        }

        // 先頭1文字だけ使う
        // Substring(x, y) ⇒ x文字目のy番目の文字を取り出す
        _myAlphabet = _myAlphabet.Substring(0, 1).ToUpper();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // 黒くする
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = Color.gray;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // トン
    public void Dit(int[] array, int index)
    {
        // 隣のノードのトンツーを呼ぶ
        if (array[index + 1] == 1) // トン
        {
            _ditNode.Dit(array, index + 1);
            _ditEdge.SetRoute(true);
        }
        else if (array[index + 1] == 2) // ツー
        {
            _dahNode.Dah(array, index + 1);
            _dahEdge.SetRoute(true);
        }
        else if (array[index + 1] == 0) // 終端文字≒自身が目的の文字
        {
            // 明るくする
            FlashLight();
        }
    }
    // ツー
    public void Dah(int[] array, int index)
    {
        // 隣のノードのトンツーを呼ぶ
        if (array[index + 1] == 1) // トン
        {
            _ditNode.Dit(array, index + 1);
            _ditEdge.SetRoute(true);
        }
        else if (array[index + 1] == 2) // ツー
        {
            _dahNode.Dah(array, index + 1);
            _dahEdge.SetRoute(true);
        }
        else if (array[index + 1] == 0) // 終端文字≒自身が目的の文字
        {
            // 明るくする
            FlashLight();
        }
    }

    // 自身が正解のルートか返す
    public bool IsRightNode()
    {
        return _prevEdge.IsRoute();
    }

    // 色を変える
    private void FlashLight()
    {
        //Debug.Log("kokodayo: " + _myAlphabet);

        // 明るくする
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = Color.white;

        // フラグ立て
        _isAnswerNode = true;
    }
    // 色を暗くする
    public void OffLight()
    {
        // 明るくする
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = Color.gray;

        // フラグ折る
        _isAnswerNode = false;
    }

    // ノードのアニメーションを止める
    public void StopAnimationEdge()
    {
        if (_prevEdge == null) return;

        _prevEdge.ResetEdgeAnimation();
        _prevNode.StopAnimationEdge(); // 再帰呼び出し
    }

}
