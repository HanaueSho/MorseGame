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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
