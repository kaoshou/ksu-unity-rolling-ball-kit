using System.Collections;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{
    [Header("崩塌設定")]
    [Tooltip("踩上平台後，多久會塌陷消失 (秒)")]
    public float crumbleDelay = 1.5f;

    [Tooltip("崩塌消失後，多久會重生出現 (秒)")]
    public float respawnDelay = 3f;

    [Tooltip("震動幅度 (數值越大震動越劇烈，做為塌陷前的視覺提示)")]
    public float shakeAmount = 0.05f;

    [Tooltip("玩家的標籤")]
    public string playerTag = "Player";

    private Vector3 originalLocalPos;
    private MeshRenderer meshRenderer;
    private Collider platformCollider;
    private bool isTriggered = false;

    void Start()
    {
        // 記錄初始位置，以便震動與重生時歸位
        originalLocalPos = transform.localPosition;
        meshRenderer = GetComponent<MeshRenderer>();
        platformCollider = GetComponent<Collider>();
    }

    // 當玩家踩上實體平台時觸發 (此物件的 Collider 的 Is Trigger 必須取消勾選)
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(playerTag) && !isTriggered)
        {
            StartCoroutine(CrumbleSequence());
        }
    }

    private IEnumerator CrumbleSequence()
    {
        isTriggered = true;

        // 1. 震動階段 (提供即將塌陷的視覺回饋)
        float elapsed = 0f;
        while (elapsed < crumbleDelay)
        {
            // 隨機產生微小的 X 與 Z 軸位移
            float shakeX = Random.Range(-shakeAmount, shakeAmount);
            float shakeZ = Random.Range(-shakeAmount, shakeAmount);
            transform.localPosition = originalLocalPos + new Vector3(shakeX, 0f, shakeZ);
            
            elapsed += Time.deltaTime;
            yield return null; // 等待下一幀
        }

        // 確保震動結束後位置歸位
        transform.localPosition = originalLocalPos;

        // 2. 塌陷隱藏階段 (關閉網格渲染與物理碰撞體)
        if (meshRenderer != null) meshRenderer.enabled = false;
        if (platformCollider != null) platformCollider.enabled = false;

        // 3. 等待重生時間
        yield return new WaitForSeconds(respawnDelay);

        // 4. 重生階段 (重新開啟網格與碰撞，並將狀態重設)
        if (meshRenderer != null) meshRenderer.enabled = true;
        if (platformCollider != null) platformCollider.enabled = true;
        isTriggered = false;
    }
}
