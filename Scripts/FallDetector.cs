using UnityEngine;
using UnityEngine.SceneManagement;

public class FallDetector : MonoBehaviour
{
    [Header("墜落高度限制")]
    [Tooltip("當玩家球體的 Y 座標低於此數值時，判定為墜落死亡。")]
    public float fallLimitY = -10f;

    [Header("掉落後動作")]
    [Tooltip("勾選此項：墜落後直接重新載入場景 (關卡重來)")]
    public bool reloadScene = true;

    [Tooltip("不重新載入場景時，玩家要傳送復活的空物件位置 (需要取消勾選 Reload Scene)")]
    public Transform respawnPoint;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 當球體 Y 座標小於設定 of 界線時，觸發死亡重置
        if (transform.position.y < fallLimitY)
        {
            TriggerReset();
        }
    }

    private void TriggerReset()
    {
        if (reloadScene)
        {
            // 重新載入當前活動場景
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            // 優先使用動態記錄點，若無則使用手動設定的復活點
            Transform targetRespawn = Checkpoint.CurrentCheckpoint != null ? Checkpoint.CurrentCheckpoint : respawnPoint;

            if (targetRespawn != null)
            {
                // 傳送回指定復活點
                transform.position = targetRespawn.position;
                
                // 清除 Rigidbody 的速度與旋轉慣性
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
            }
            else
            {
                Debug.LogWarning("FallDetector: 未指定任何復活位置且未勾選 Reload Scene！");
            }
        }
    }

    // 當在編輯器中選取掛載此腳本的物件時，在 Scene 視窗繪製輔助線
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        // 繪製一個代表死亡界線的黃色輔助平面框
        Vector3 center = new Vector3(transform.position.x, fallLimitY, transform.position.z);
        Gizmos.DrawWireCube(center, new Vector3(10f, 0.1f, 10f));
    }
}
