using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("追蹤目標")]
    [Tooltip("要追蹤的目標 (通常是玩家的球體)。若不拖入，系統會自動尋找 Tag 為 Player 的物件。")]
    public Transform target;

    [Header("相機偏移量")]
    [Tooltip("相機與主角的相對距離與角度。若設為 (0,0,0)，遊戲開始時會自動計算目前的距離作為偏移量。")]
    public Vector3 offset = Vector3.zero;

    void Start()
    {
        // 防呆設計：如果沒有在 Inspector 拖入 target，自動搜尋場景中帶有 "Player" Tag 的物件
        if (target == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
            else
            {
                Debug.LogWarning("CameraFollow: 場景中找不到 Tag 為 'Player' 的物件，請手動指派追蹤目標！");
            }
        }

        // 如果偏移量設為零，自動以目前的相對位置作為偏移量
        if (offset == Vector3.zero && target != null)
        {
            offset = transform.position - target.position;
        }
    }

    void LateUpdate()
    {
        // 如果沒有追蹤目標，則不執行相機移動
        if (target == null) return;

        // 硬跟隨：相機位置 = 主角位置 + 固定偏移量
        transform.position = target.position + offset;
    }
}