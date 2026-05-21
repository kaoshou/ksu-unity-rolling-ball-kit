using System.Collections;
using UnityEngine;

public class SpeedBoostItem : MonoBehaviour
{
    [Header("加速力道與方向")]
    [Tooltip("加速的力道大小 (會直接對玩家施加瞬間速度改變)")]
    public float boostForce = 20f;

    [Tooltip("如果非相對方向，加速的世界座標方向")]
    public Vector3 boostDirection = Vector3.forward;

    [Tooltip("是否以這個加速道具的前方 (Local Forward) 作為衝刺方向 (推薦勾選，旋轉道具就能改變衝刺方向)")]
    public bool useLocalDirection = true;

    [Header("冷卻與玩家標籤")]
    [Tooltip("加速道具的冷卻時間 (秒)。若小於等於 0，則吃掉後會直接銷毀，不再產生。")]
    public float cooldownTime = 3f;

    [Tooltip("玩家的標籤")]
    public string playerTag = "Player";

    private MeshRenderer meshRenderer;
    private Collider itemCollider;
    private bool isReady = true;

    void Start()
    {
        // 取得自身的渲染器與碰撞器，以便進行隱藏與重置
        meshRenderer = GetComponent<MeshRenderer>();
        itemCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 如果還在冷卻中則不執行
        if (!isReady) return;

        // 檢查碰到的物件是否是玩家
        if (other.CompareTag(playerTag))
        {
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // 計算力道方向
                Vector3 forceDirection = useLocalDirection ? transform.forward : boostDirection.normalized;

                // 施加瞬間速度改變 (VelocityChange 會忽略質量，直接賦予速度增量)
                playerRb.AddForce(forceDirection * boostForce, ForceMode.VelocityChange);

                // 開始冷卻程序
                StartCoroutine(StartCooldown());
            }
        }
    }

    private IEnumerator StartCooldown()
    {
        isReady = false;

        // 隱藏道具外觀與關閉碰撞
        if (meshRenderer != null) meshRenderer.enabled = false;
        if (itemCollider != null) itemCollider.enabled = false;

        // 判斷是要冷卻重設，還是直接銷毀
        if (cooldownTime > 0)
        {
            // 等待冷卻時間結束
            yield return new WaitForSeconds(cooldownTime);
            
            // 重新顯示道具與開啟碰撞
            isReady = true;
            if (meshRenderer != null) meshRenderer.enabled = true;
            if (itemCollider != null) itemCollider.enabled = true;
        }
        else
        {
            // 一次性道具，直接銷毀
            Destroy(gameObject);
        }
    }
}
