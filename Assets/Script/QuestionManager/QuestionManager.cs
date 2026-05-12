using System.Collections.Generic;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    [Header("プレイヤー参照")]
    public PlayerStateManager _player;
    public AlphabetStateManager _rootNode; // これいらないかも

    [Header("問題集")]
    public string[] _questions; // 問題集

    [Header("現在出題中の問題")]
    public string[] _nowQuestion; // 現在の問題

    [Header("出題中の問題のモールス記号")]
    [SerializeField]private int[] _nowArray; // 現在の文字
    [SerializeField]private int _nowIndex = 0; // 現在の文字のインデックス


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 問題を設定
        _nowQuestion = new string[_questions[0].Length];
        for (int i = 0; i < _questions[0].Length; i++)
        {
            _nowQuestion[i] = _questions[0][i].ToString();
        }

        _nowIndex = 0;
        // 配列へ変換
        _nowArray = ConvertMorseToArray(_nowQuestion[0]);
        // 配列からアルファベットを光らせる
        _player.PrepareQuestionAlphabet(_nowArray);
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.IsGetAnswerNode())
        {
            _nowIndex++;
            if (_nowIndex >= _nowQuestion.Length)
            {
                _nowIndex = 0;
            }
            Debug.Log("nowIndex: " + _nowIndex);
            _nowArray = ConvertMorseToArray(_nowQuestion[_nowIndex]);
            _player.PrepareQuestionAlphabet(_nowArray);
        }
    }




    // 文字列から数字の配列へ
    // トン　⇒　１
    // ツー　⇒　２
    // 空き　⇒　０
    int[] ConvertMorseToArray(string alphabet)
    {
        // Dictionary 型
        Dictionary<string, int[]> morseMap = new Dictionary<string, int[]>()
    {
        { "A", new int[] { 1, 2, 0, 0, 0 } }, // ・－
        { "B", new int[] { 2, 1, 1, 1, 0 } }, // －・・・
        { "C", new int[] { 2, 1, 2, 1, 0 } }, // －・－・
        { "D", new int[] { 2, 1, 1, 0, 0 } }, // －・・
        { "E", new int[] { 1, 0, 0, 0, 0 } }, // ・
        { "F", new int[] { 1, 1, 2, 1, 0 } }, // ・・－・
        { "G", new int[] { 2, 2, 1, 0, 0 } }, // －－・
        { "H", new int[] { 1, 1, 1, 1, 0 } }, // ・・・・
        { "I", new int[] { 1, 1, 0, 0, 0 } }, // ・・
        { "J", new int[] { 1, 2, 2, 2, 0 } }, // ・－－－
        { "K", new int[] { 2, 1, 2, 0, 0 } }, // －・－
        { "L", new int[] { 1, 2, 1, 1, 0 } }, // ・－・・
        { "M", new int[] { 2, 2, 0, 0, 0 } }, // －－
        { "N", new int[] { 2, 1, 0, 0, 0 } }, // －・
        { "O", new int[] { 2, 2, 2, 0, 0 } }, // －－－
        { "P", new int[] { 1, 2, 2, 1, 0 } }, // ・－－・
        { "Q", new int[] { 2, 2, 1, 2, 0 } }, // －－・－
        { "R", new int[] { 1, 2, 1, 0, 0 } }, // ・－・
        { "S", new int[] { 1, 1, 1, 0, 0 } }, // ・・・
        { "T", new int[] { 2, 0, 0, 0, 0 } }, // －
        { "U", new int[] { 1, 1, 2, 0, 0 } }, // ・・－
        { "V", new int[] { 1, 1, 1, 2, 0 } }, // ・・・－
        { "W", new int[] { 1, 2, 2, 0, 0 } }, // ・－－
        { "X", new int[] { 2, 1, 1, 2, 0 } }, // －・・－
        { "Y", new int[] { 2, 1, 2, 2, 0 } }, // －・－－
        { "Z", new int[] { 2, 2, 1, 1, 0 } }, // －－・・
    };
        // もしも引数の文字列無しだった場合対策
        if (string.IsNullOrEmpty(alphabet))
        {
            return new int[] { 0, 0, 0, 0, 0 };
        }

        // １文字の大文字に変換しておく
        string alpha = alphabet.Substring(0, 1).ToUpper();

        if (morseMap.ContainsKey(alpha))
        {
            return morseMap[alpha];
        }

        return new int[] { 0, 0, 0, 0, 0 };
    }

}
