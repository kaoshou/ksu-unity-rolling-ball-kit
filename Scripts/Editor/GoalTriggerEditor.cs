using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GoalTrigger))]
[CanEditMultipleObjects]
public class GoalTriggerEditor : Editor
{
    private SerializedProperty textTypeProp;
    private SerializedProperty winTextTMPProp;
    private SerializedProperty winTextLegacyProp;
    private SerializedProperty winMessageProp;
    private SerializedProperty loadNextLevelProp;
    private SerializedProperty nextSceneNameProp;
    private SerializedProperty loadDelayProp;
    private SerializedProperty playerTagProp;

    private void OnEnable()
    {
        // 取得屬性關聯，以利在 Inspector 繪製
        textTypeProp = serializedObject.FindProperty("textType");
        winTextTMPProp = serializedObject.FindProperty("winTextTMP");
        winTextLegacyProp = serializedObject.FindProperty("winTextLegacy");
        winMessageProp = serializedObject.FindProperty("winMessage");
        loadNextLevelProp = serializedObject.FindProperty("loadNextLevel");
        nextSceneNameProp = serializedObject.FindProperty("nextSceneName");
        loadDelayProp = serializedObject.FindProperty("loadDelay");
        playerTagProp = serializedObject.FindProperty("playerTag");
    }

    public override void OnInspectorGUI()
    {
        // 更新序列化物件狀態
        serializedObject.Update();

        GUILayout.Space(5);
        GUILayout.Label("🏁 終點 UI 文字配置", EditorStyles.boldLabel);

        // 1. 繪製 UI 類型的下拉選單
        EditorGUILayout.PropertyField(textTypeProp, new GUIContent("UI 文字類型", "請選擇您要使用的 UI 文字組件類型"));

        // 2. 根據選單的值，動態繪製對應的公開變數欄位
        GoalTrigger.TextType selectedType = (GoalTrigger.TextType)textTypeProp.enumValueIndex;

        if (selectedType == GoalTrigger.TextType.TextMeshPro)
        {
            EditorGUILayout.PropertyField(winTextTMPProp, new GUIContent("Win Text (TMP)", "將畫面上建立的 TextMeshProUGUI 物件拖入此處"));
        }
        else if (selectedType == GoalTrigger.TextType.LegacyText)
        {
            EditorGUILayout.PropertyField(winTextLegacyProp, new GUIContent("Win Text (Legacy)", "將畫面上建立的 Legacy Text 物件拖入此處"));
        }

        GUILayout.Space(5);
        // 3. 繪製其他通用變數
        EditorGUILayout.PropertyField(winMessageProp, new GUIContent("通關顯示文字"));
        
        GUILayout.Space(5);
        GUILayout.Label("🚪 關卡轉場設定", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(loadNextLevelProp, new GUIContent("載入下一關", "是否在觸發終點後載入新關卡"));
        if (loadNextLevelProp.boolValue)
        {
            EditorGUILayout.PropertyField(nextSceneNameProp, new GUIContent("指定下一關名稱", "欲載入的場景名稱。如果留空，將會按 Build Settings 順序載入下一個場景"));
            EditorGUILayout.PropertyField(loadDelayProp, new GUIContent("載入延遲時間 (秒)", "顯示通關文字後，過多久才轉場"));
        }

        GUILayout.Space(5);
        EditorGUILayout.PropertyField(playerTagProp, new GUIContent("玩家標籤 (Tag)"));

        // 套用所有修改的屬性值
        serializedObject.ApplyModifiedProperties();
    }
}
