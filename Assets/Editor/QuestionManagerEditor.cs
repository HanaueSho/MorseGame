using UnityEditor;      // Editor拡張用。CustomEditor、AssetDatabase、SerializedObjectなどを使うために必要
using UnityEngine;      // Unityの基本機能。Sprite、GUILayout、Debugなどを使うために必要
using System.Linq;      // Select、Where、OrderBy、FirstOrDefaultなどのLINQを使うために必要

// QuestionManager用のカスタムインスペクタを作る指定
// これにより、QuestionManagerが付いたGameObjectを選択した時のInspector表示を拡張できる
[CustomEditor(typeof(QuestionManager))]
public class QuestionManagerEditor : Editor
{
    // Inspectorの表示内容を上書きするメソッド
    public override void OnInspectorGUI()
    {
        // 通常のInspector表示をそのまま描画する
        // QuestionManagerの[SerializeField]などはここで表示される
        DrawDefaultInspector();

        // 現在Inspectorで選択されているQuestionManagerを取得する
        QuestionManager manager = (QuestionManager)target;

        // Inspectorにボタンを表示する
        // ボタンが押された瞬間だけtrueになる
        if (GUILayout.Button("Load Alphabet Sprites"))
        {
            // ボタンが押されたら、指定フォルダからスプライトを読み込んで
            // QuestionManagerの_alphabetSpritesに自動設定する
            LoadSprites(manager);
        }
    }

    // 指定フォルダ内のアルファベット用Spriteを探して、
    // QuestionManagerの_alphabetSprites配列にA~Zの順番で入れるメソッド
    private void LoadSprites(QuestionManager manager)
    {
        // Spriteを探す対象フォルダ
        // 実際のプロジェクト内のパスと完全に一致している必要がある
        // 例: Assets/Sprite/Alphabet/A.png
        string folderPath = "Assets/Sprite/Alphabet";

        // 指定フォルダ内からSprite型のアセットを検索する
        // 戻り値はアセットそのものではなくGUIDという内部ID
        string[] guids = AssetDatabase.FindAssets("t:Sprite", new[] { folderPath });

        // GUIDからアセットパスに変換し、
        // そのパスからSpriteを読み込み、
        // nullを除外し、
        // スプライト名順に並べ替えて配列にする
        Sprite[] sprites = guids
            .Select(guid => AssetDatabase.GUIDToAssetPath(guid))          // GUID -> アセットパス
            .Select(path => AssetDatabase.LoadAssetAtPath<Sprite>(path))  // パス -> Sprite
            .Where(sprite => sprite != null)                              // 読み込み失敗したものを除外
            .OrderBy(sprite => sprite.name)                               // 名前順に並べ替え
            .ToArray();                                                   // 配列化

        // QuestionManagerをSerializedObjectとして扱う
        // privateな[SerializeField]フィールドを安全に編集するために使う
        SerializedObject serializedObject = new SerializedObject(manager);

        // QuestionManager内の_alphabetSpritesフィールドを取得する
        // フィールド名が違うとnullになるので注意
        SerializedProperty property = serializedObject.FindProperty("_alphabetSprites");

        // _alphabetSpritesが見つからなかった場合の安全対策
        if (property == null)
        {
            Debug.LogError("QuestionManagerに _alphabetSprites というSerializedFieldが見つかりません。");
            return;
        }

        // 配列サイズを26にする
        // A~Zの26文字分
        property.arraySize = 26;

        // A~Zまで順番に処理する
        for (int i = 0; i < 26; i++)
        {
            // i = 0なら'A'、i = 1なら'B' ... i = 25なら'Z'
            char letter = (char)('A' + i);

            // スプライト名が "A", "B", "C" ... と完全一致するものを探す
            Sprite sprite = sprites.FirstOrDefault(s => s.name == letter.ToString());

            // _alphabetSprites[i] に見つかったSpriteを設定する
            // 見つからなかった場合はnullが入る
            property.GetArrayElementAtIndex(i).objectReferenceValue = sprite;

            // 見つからなかった場合は警告を出す
            if (sprite == null)
            {
                Debug.LogWarning($"{letter} のSpriteが見つかりませんでした。Sprite名を確認してください。");
            }
        }

        // SerializedObjectへの変更を実際のQuestionManagerに反映する
        serializedObject.ApplyModifiedProperties();

        // このオブジェクトが変更されたことをUnity Editorに知らせる
        // これにより、シーン保存時などに変更が失われにくくなる
        EditorUtility.SetDirty(manager);

        // 完了ログ
        Debug.Log("Alphabet sprites loaded.");
    }
}