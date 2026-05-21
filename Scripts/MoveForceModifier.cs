using System.Collections;
using UnityEngine;

public class MoveForceModifier : MonoBehaviour
{
    [Header("力道調整設定")]
    [Tooltip("玩家移動力道的倍率 (例如：2 代表力道變為 2 倍/加速，0.5 代表力道減半/減速)")]
    public float forceMultiplier = 2f;

    [Tooltip("效果持續時間 (秒)。若設為 0 或負數，則為永久改變，直到玩家陣亡或重載關卡")]
    public float duration = 5f;

    [Tooltip("道具被吃掉後，隔多久會重新出現 (秒)。若勾選『一次性道具』則此欄位無效")]
    public float respawnDelay = 3f;

    [Tooltip("勾選此項：道具被吃掉後會永久銷毀消失，不會再次重生")]
    public bool destroyAfterUse = false;

    [Tooltip("玩家的標籤")]
    public string playerTag = "Player";

    private Collider myCollider;
    private Renderer myRenderer;
    private bool isCollected = false;

    void Start()
    {
        myCollider = GetComponent<Collider>();
        myRenderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 偵測是否碰撞到玩家，且目前沒有處於冷卻狀態
        if (other.CompareTag(playerTag) && !isCollected)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                StartCoroutine(ApplyModifierSequence(player));
            }
        }
    }

    private IEnumerator ApplyModifierSequence(PlayerController player)
    {
        isCollected = true;

        // 1. 隱藏道具 (關閉碰撞與渲染)
        if (myCollider != null) myCollider.enabled = false;
        if (myRenderer != null) myRenderer.enabled = false;

        // 2. 記錄原本的力道並套用新的力道
        float originalForce = player.moveForce;
        player.moveForce = originalForce * forceMultiplier;

        // 3. 處理持續時間
        if (duration > 0f)
        {
            // 等待道具時效結束
            yield return new WaitForSeconds(duration);

            // 還原玩家移動力道 (如果玩家物件未被銷毀)
            if (player != null)
            {
                player.moveForce = originalForce;
            }
        }

        // 4. 判斷是否為一次性道具
        if (destroyAfterUse)
        {
            Destroy(gameObject);
        }
        else
        {
            // 等待冷卻時間，然後重生道具
            yield return new WaitForSeconds(respawnDelay);

            isCollected = false;
            if (myCollider != null) myCollider.enabled = true;
            if (myRenderer != null) myRenderer.enabled = true;
        }
    }
}
