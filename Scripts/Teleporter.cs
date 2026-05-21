using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [Header("傳送設定")]
    [Tooltip("傳送的目的地 (通常是另一個傳送門的 Transform)")]
    public Transform destination;

    [Tooltip("是否保留球體原本的物理速度")]
    public bool keepVelocity = true;

    [Tooltip("玩家的標籤")]
    public string playerTag = "Player";

    // 靜態全域變數，用來防止兩個傳送門靠太近時，球體在兩者之間產生無限傳送的死循環
    private static float nextAllowedTeleportTime = 0f;

    // 使用 OnTriggerEnter 來觸發傳送門 (此物件的 Collider 必須勾選 Is Trigger)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            // 檢查是否還在傳送冷卻期間，避免死循環
            if (Time.time < nextAllowedTeleportTime) return;

            if (destination != null)
            {
                // 設定傳送冷卻間隔 (0.5 秒內不允許再次傳送)
                nextAllowedTeleportTime = Time.time + 0.5f;

                // 傳送主角球體的位置到目的地
                other.transform.position = destination.position;

                // 處理物理速度
                Rigidbody rb = other.GetComponent<Rigidbody>();
                if (rb != null && !keepVelocity)
                {
                    // 若不保留速度，則清除滾動的慣性速度與旋轉力，防止飛出
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
            }
            else
            {
                Debug.LogWarning("Teleporter: 未設定傳送目的地 (Destination)！");
            }
        }
    }
}
