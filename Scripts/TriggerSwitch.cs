using UnityEngine;

public class TriggerSwitch : MonoBehaviour
{
    public enum SwitchAction
    {
        ToggleShowHide,   // 切換目標物件的 顯示/隱藏
        ShowObject,       // 顯示目標物件
        HideObject,       // 隱藏目標物件
        StartMovement,    // 啟動移動平台/移動障礙物
        StopMovement,     // 停止移動平台/移動障礙物
        ToggleMovement    // 切換移動平台/移動障礙物的啟動狀態
    }

    [Header("觸發設定")]
    [Tooltip("觸發此開關後要執行的行為")]
    public SwitchAction action = SwitchAction.ToggleShowHide;

    // 以下變數會在自訂編輯器 (TriggerSwitchEditor) 中根據 action 動態顯示，避免介面過於複雜
    [HideInInspector]
    public GameObject targetObject;

    [HideInInspector]
    public PathMovement targetMovement;

    [Tooltip("是否只允許觸發一次")]
    public bool triggerOnce = false;

    [Tooltip("觸發後是否讓開關物件本身消失 (銷毀此開關)")]
    public bool destroySelfAfterTrigger = false;

    [Tooltip("觸發冷卻時間 (秒)")]
    public float cooldown = 0.5f;

    [Tooltip("玩家的標籤")]
    public string playerTag = "Player";

    [Header("開關視覺回饋 (選填)")]
    [Tooltip("被觸發時，開關是否要改變自身的材質顏色")]
    public bool changeColor = true;
    public Color activeColor = Color.green;

    private Color originalColor;
    private Renderer myRenderer;
    private bool isTriggered = false;
    private float cooldownTimer = 0f;

    void Start()
    {
        myRenderer = GetComponent<Renderer>();
        if (myRenderer != null && changeColor)
        {
            originalColor = myRenderer.material.color;
        }
    }

    void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    // 適用於觸發區域 (Is Trigger 勾選)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            TryTrigger();
        }
    }

    // 適用於實體按鈕/碰撞開關
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            TryTrigger();
        }
    }

    private void TryTrigger()
    {
        // 限制：冷卻中，或是單次觸發且已被觸發過
        if (cooldownTimer > 0f) return;
        if (triggerOnce && isTriggered) return;

        isTriggered = true;
        cooldownTimer = cooldown;

        // 視覺顏色改變
        if (myRenderer != null && changeColor)
        {
            myRenderer.material.color = activeColor;
        }

        // 執行機關邏輯
        ExecuteAction();

        // 若開啟觸發後消失，則銷毀此開關物件
        if (destroySelfAfterTrigger)
        {
            Destroy(gameObject);
        }
    }

    private void ExecuteAction()
    {
        // 1. 處理目標物件顯示/隱藏
        if (targetObject != null)
        {
            if (action == SwitchAction.ShowObject)
            {
                targetObject.SetActive(true);
            }
            else if (action == SwitchAction.HideObject)
            {
                targetObject.SetActive(false);
            }
            else if (action == SwitchAction.ToggleShowHide)
            {
                targetObject.SetActive(!targetObject.activeSelf);
            }
        }

        // 2. 處理移動平台的開始/停止
        if (targetMovement != null)
        {
            if (action == SwitchAction.StartMovement)
            {
                targetMovement.Activate();
            }
            else if (action == SwitchAction.StopMovement)
            {
                targetMovement.Deactivate();
            }
            else if (action == SwitchAction.ToggleMovement)
            {
                targetMovement.ToggleActivate();
            }
        }
    }

    // 供外部重置開關顏色的方法
    public void ResetSwitch()
    {
        isTriggered = false;
        if (myRenderer != null && changeColor)
        {
            myRenderer.material.color = originalColor;
        }
    }
}
