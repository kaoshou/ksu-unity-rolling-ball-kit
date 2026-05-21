using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TriggerSwitch))]
[CanEditMultipleObjects]
public class TriggerSwitchEditor : Editor
{
    private SerializedProperty actionProp;
    private SerializedProperty targetObjectProp;
    private SerializedProperty targetMovementProp;
    private SerializedProperty triggerOnceProp;
    private SerializedProperty cooldownProp;
    private SerializedProperty playerTagProp;
    private SerializedProperty changeColorProp;
    private SerializedProperty activeColorProp;

    private SerializedProperty destroySelfAfterTriggerProp;

    private void OnEnable()
    {
        actionProp = serializedObject.FindProperty("action");
        targetObjectProp = serializedObject.FindProperty("targetObject");
        targetMovementProp = serializedObject.FindProperty("targetMovement");
        triggerOnceProp = serializedObject.FindProperty("triggerOnce");
        destroySelfAfterTriggerProp = serializedObject.FindProperty("destroySelfAfterTrigger");
        cooldownProp = serializedObject.FindProperty("cooldown");
        playerTagProp = serializedObject.FindProperty("playerTag");
        changeColorProp = serializedObject.FindProperty("changeColor");
        activeColorProp = serializedObject.FindProperty("activeColor");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUILayout.Space(5);
        GUILayout.Label("🔌 機關開關設定", EditorStyles.boldLabel);

        // 1. 繪製開關觸發動作選單
        EditorGUILayout.PropertyField(actionProp, new GUIContent("觸發行為", "當玩家碰到開關時要執行的動作"));

        TriggerSwitch.SwitchAction selectedAction = (TriggerSwitch.SwitchAction)actionProp.enumValueIndex;

        // 2. 根據觸發行為動態顯示對應的目標物件欄位
        if (selectedAction == TriggerSwitch.SwitchAction.ShowObject ||
            selectedAction == TriggerSwitch.SwitchAction.HideObject ||
            selectedAction == TriggerSwitch.SwitchAction.ToggleShowHide)
        {
            EditorGUILayout.PropertyField(targetObjectProp, new GUIContent("控制目標物件", "要進行顯示/隱藏的 GameObject"));
        }
        else if (selectedAction == TriggerSwitch.SwitchAction.StartMovement ||
                 selectedAction == TriggerSwitch.SwitchAction.StopMovement ||
                 selectedAction == TriggerSwitch.SwitchAction.ToggleMovement)
        {
            EditorGUILayout.PropertyField(targetMovementProp, new GUIContent("控制移動平台", "掛載了 PathMovement 腳本的平台或障礙物"));
        }

        GUILayout.Space(10);
        GUILayout.Label("⚙️ 觸發規則與回饋", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(triggerOnceProp, new GUIContent("只觸發一次", "勾選後此開關在遊戲中只能被按下一次"));
        EditorGUILayout.PropertyField(destroySelfAfterTriggerProp, new GUIContent("觸發後本身消失", "勾選後此開關被碰觸後會自動銷毀消失"));
        EditorGUILayout.PropertyField(cooldownProp, new GUIContent("觸發冷卻時間 (秒)"));
        EditorGUILayout.PropertyField(playerTagProp, new GUIContent("玩家標籤 (Tag)"));

        GUILayout.Space(5);
        EditorGUILayout.PropertyField(changeColorProp, new GUIContent("觸發時改變顏色", "是否在被碰到時改變開關自身的材質顏色"));

        if (changeColorProp.boolValue)
        {
            EditorGUILayout.PropertyField(activeColorProp, new GUIContent("觸發後的顏色"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
