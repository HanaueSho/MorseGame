using UnityEngine;

public class AlphabetStateManager : MonoBehaviour
{
    // 自分のアルファベット
    [SerializeField] private string _myAlphabet = "A";
    public string MyAlphabet => _myAlphabet;

    // 隣り合うアルファベットの参照
    [SerializeField] private AlphabetStateManager _ditNode; // トン
    [SerializeField] private AlphabetStateManager _dahNode; // ツー
    public AlphabetStateManager DitNode => _ditNode;
    public AlphabetStateManager DahNode => _dahNode;

    // 隣り合うアルファベットへのエッジの参照
    [SerializeField] private AlphabetStateManager _ditEdge; // トン
    [SerializeField] private AlphabetStateManager _dahEdge; // ツー

    // 目的のノードかフラグ
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

    // QuestionManager から呼ばれる
    public void PrepareQuestionAlphabet(int[] array)
    {
        // １文字目のトンツーを呼ぶ
        if (array[0] == 1) // トン
        {
            _ditNode.Dit(array, 0);
        }
        else if(array[0] == 2) // ツー
        {
            _dahNode.Dah(array, 0);
        }
    }

    // トン
    public void Dit(int[] array, int index)
    {
        // 隣のノードのトンツーを呼ぶ
        if (array[index + 1] == 1) // トン
        {
            _ditNode.Dit(array, index + 1);
        }
        else if (array[index + 1] == 2) // ツー
        {
            _dahNode.Dah(array, index + 1);
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
        }
        else if (array[index + 1] == 2) // ツー
        {
            _dahNode.Dah(array, index + 1);
        }
        else if (array[index + 1] == 0) // 終端文字≒自身が目的の文字
        {
            // 明るくする
            FlashLight();
        }
    }

    // 色を変える
    private void FlashLight()
    {
        Debug.Log("kokodayo: " + _myAlphabet);

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
}
