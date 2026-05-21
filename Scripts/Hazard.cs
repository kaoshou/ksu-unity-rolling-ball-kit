using UnityEngine;
using UnityEngine.SceneManagement;

public class Hazard : MonoBehaviour
{
    [Header("重置設定")]
    [Tooltip("勾選時，碰到此危險物會直接重新載入場景。")]
    public bool reloadScene = true;

    [Tooltip("不重新載入場景時，玩家要傳送復活的空物件位置 (需要取消勾選 Reload Scene)")]
    public Transform respawnPoint;

    [Header("玩家設定")]
    [Tooltip("玩家的 Tag 標籤，預設為 Player")]
    public string playerTag = "Player";

    // 1. 適用於實體碰撞 (例如：尖刺、移動障礙牆、滾石)
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            ExecuteReset(collision.gameObject);
        }
    }

    // 2. 適用於穿透型觸發器 (例如：雷射光束、毒氣區、岩漿表面)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            ExecuteReset(other.gameObject);
        }
    }

    // 執行死亡重置或傳送復活
    private void ExecuteReset(GameObject player)
    {
        if (reloadScene)
        {
            // 重新載入場景
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            // 優先使用動態記錄點，若無則使用手動設定的復活點
            Transform targetRespawn = Checkpoint.CurrentCheckpoint != null ? Checkpoint.CurrentCheckpoint : respawnPoint;

            if (targetRespawn != null)
            {
                // 傳送回指定點並重設物理速度
                player.transform.position = targetRespawn.position;
                Rigidbody rb = player.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
            }
            else
            {
                Debug.LogWarning("Hazard: 未設定任何復活位置且未勾選 reloadScene！");
            }
        }
    }
}
