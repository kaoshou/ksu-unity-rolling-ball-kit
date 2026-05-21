using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [Header("彈跳設定")]
    [Tooltip("向上彈跳的力道大小 (會直接覆蓋玩家的垂直速度)")]
    public float jumpForce = 15f;

    [Tooltip("玩家的標籤")]
    public string playerTag = "Player";

    // 使用 OnTriggerEnter 偵測觸發，確保滾動球體在碰觸時能獲得平滑且穩定的向上推力
    private void OnTriggerEnter(Collider other)
    {
        // 檢查觸發的是否是玩家
        if (other.CompareTag(playerTag))
        {
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // 取得目前速度
                Vector3 currentVelocity = playerRb.linearVelocity;
                
                // 直接將 Y 軸垂直速度設定為跳躍力道，確保不受原本下落速度的影響，彈起高度一致
                currentVelocity.y = jumpForce;
                playerRb.linearVelocity = currentVelocity;
            }
        }
    }
}
