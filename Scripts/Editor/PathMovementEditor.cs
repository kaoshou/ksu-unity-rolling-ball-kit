using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathMovement))]
public class PathMovementEditor : Editor
{
    private void OnSceneGUI()
    {
        PathMovement mover = (PathMovement)target;
        if (mover.localPoints == null || mover.localPoints.Length == 0) return;

        Transform parentTransform = mover.transform.parent;

        // 當使用者在場景中拖拉控制點時，記錄 Undo 動作，方便 Ctrl+Z 還原
        Undo.RecordObject(mover, "Modify Path Points");

        for (int i = 0; i < mover.localPoints.Length; i++)
        {
            // 將絕對本機座標轉換為世界空間位置
            Vector3 worldPos = parentTransform != null ? parentTransform.TransformPoint(mover.localPoints[i]) : mover.localPoints[i];

            // 繪製路徑點名稱與球體
            Handles.color = mover.pathColor;
            Handles.SphereHandleCap(0, worldPos, Quaternion.identity, 0.2f, EventType.Repaint);
            Handles.Label(worldPos + Vector3.up * 0.3f, "點 " + i, new GUIStyle() { normal = { textColor = mover.pathColor }, fontStyle = FontStyle.Bold });

            // 產生 3D 移動手把 (Position Handle)
            EditorGUI.BeginChangeCheck();
            Vector3 newWorldPos = Handles.PositionHandle(worldPos, Quaternion.identity);
            
            if (EditorGUI.EndChangeCheck())
            {
                // 將新的世界座標轉回絕對本機座標
                Vector3 newLocalPos = parentTransform != null ? parentTransform.InverseTransformPoint(newWorldPos) : newWorldPos;
                
                // 更新路徑點座標
                mover.localPoints[i] = newLocalPos;
                
                // 標記物件已被修改，確保存檔與 Scene 刷新
                EditorUtility.SetDirty(mover);
            }
        }
    }

    public override void OnInspectorGUI()
    {
        // 繪製預設的 Inspector 欄位
        DrawDefaultInspector();

        PathMovement mover = (PathMovement)target;

        GUILayout.Space(10);
        GUILayout.Label("💡 快捷工具", EditorStyles.boldLabel);

        if (GUILayout.Button("將物件『當前位置』新增為新路徑點"))
        {
            Undo.RecordObject(mover, "Add Current Position as Path Point");
            
            // 讀取當前的本機座標
            Vector3 currentLocalPos = mover.transform.localPosition;

            // 擴充陣列長度
            int currentLength = (mover.localPoints != null) ? mover.localPoints.Length : 0;
            Vector3[] newPoints = new Vector3[currentLength + 1];
            for (int i = 0; i < currentLength; i++)
            {
                newPoints[i] = mover.localPoints[i];
            }
            
            // 將物件目前所在的本機位置新增為最後一個點
            newPoints[currentLength] = currentLocalPos;
            mover.localPoints = newPoints;
            
            EditorUtility.SetDirty(mover);
            Debug.Log($"已成功將物件的當前位置 {currentLocalPos} 新增為路徑點！");
        }

        if (GUILayout.Button("重設所有路徑點 (只保留目前位置為唯一點)"))
        {
            if (EditorUtility.DisplayDialog("重設路徑點", "確定要清空其他路徑點，只保留目前的物件位置作為起點嗎？", "確定", "取消"))
            {
                Undo.RecordObject(mover, "Reset Path Points");
                mover.localPoints = new Vector3[] { mover.transform.localPosition };
                EditorUtility.SetDirty(mover);
            }
        }
    }
}
