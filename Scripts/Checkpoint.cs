using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // 全域靜態變數，用來記錄玩家目前最後觸發的紀錄點
    public static Transform CurrentCheckpoint { get; private set; }

    [Header("狀態與回饋")]
    [Tooltip("被玩家碰觸啟動後，是否改變開關本身的材質顏色")]
    public bool changeColor = true;

    [Tooltip("啟動後的顏色")]
    public Color activeColor = Color.green;

    [Tooltip("玩家的標籤")]
    public string playerTag = "Player";

    private Color originalColor;
    private Renderer myRenderer;
    private bool isActive = false;

    void Start()
    {
        // 每次載入場景時，重設全域靜態紀錄點為空，防止跨局或跨關存檔殘留
        CurrentCheckpoint = null;

        myRenderer = GetComponent<Renderer>();
        if (myRenderer != null && changeColor)
        {
            originalColor = myRenderer.material.color;
        }
    }

    // 適用於穿透型觸發區域 (Collider 勾選 Is Trigger)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            ActivateCheckpoint();
        }
    }

    // 適用於實體碰撞踩踏點 (Collider 未勾選 Is Trigger)
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            ActivateCheckpoint();
        }
    }

    private void ActivateCheckpoint()
    {
        if (isActive) return;

        // 設為目前作用中的動態紀錄點
        CurrentCheckpoint = this.transform;
        isActive = true;

        // 變更目前紀錄點為啟動狀態的顏色
        if (myRenderer != null && changeColor)
        {
            myRenderer.material.color = activeColor;
        }

        // 尋找場景中其他所有紀錄點，並重置其狀態，確保同時間只有最新踩到的紀錄點是綠色的
        Checkpoint[] allCheckpoints = FindObjectsByType<Checkpoint>(FindObjectsSortMode.None);
        foreach (Checkpoint cp in allCheckpoints)
        {
            if (cp != this)
            {
                cp.ResetCheckpoint();
            }
        }
    }

    // 當有新的紀錄點被啟用時，舊的紀錄點會被重設回原本材質顏色
    public void ResetCheckpoint()
    {
        isActive = false;
        if (myRenderer != null && changeColor)
        {
            myRenderer.material.color = originalColor;
        }
    }
}
