using System.Collections;
using System.Collections.Generic; // 必須引入此命名空間以使用 List 列表
using UnityEngine;

public class PathMovement : MonoBehaviour
{
    public enum MoveType
    {
        Loop,     // 循環移動 (A -> B -> C -> A -> B...)
        PingPong, // 折返移動 (A -> B -> C -> B -> A...)
        Once      // 單次移動 (A -> B -> C，到達終點後停止)
    }

    [Header("路徑與移動設定")]
    [Tooltip("移動類型：Loop(循環)、PingPong(折返)、Once(單次)")]
    public MoveType movementType = MoveType.PingPong;

    [Tooltip("本機路徑點座標 (相對於父物件)。第一個點預設為初始位置。")]
    public Vector3[] localPoints;

    [Tooltip("移動速度")]
    public float speed = 2f;

    [Tooltip("啟動延遲時間 (秒)，適用於交錯障礙物")]
    public float startDelay = 0f;

    [Header("載客設定 (適用於移動平台)")]
    [Tooltip("玩家踩上來時，是否要讓玩家或物件跟著平台一起移動")]
    public bool carriesObjects = true;

    [Tooltip("允許被平台載動的物件標籤清單 (例如：Player, Box 等)")]
    public string[] carriedTags = new string[] { "Player" };

    [Header("機關開關設定")]
    [Tooltip("是否在遊戲開始時自動開始移動。如果取消勾選，需要透過開關機關 (TriggerSwitch) 觸發啟動。")]
    public bool autoStart = true;

    [Header("編輯器外觀")]
    [Tooltip("在 Scene 視窗中路徑輔助線的顏色 (例如平台用青綠色，障礙物用紅色)")]
    public Color pathColor = Color.cyan;

    private int targetIndex = 0;
    private bool movingForward = true;
    private bool isFinished = false;
    private float timer;

    // 用於記錄目前踩在平台上的所有物件的 Rigidbody 列表
    private List<Rigidbody> carriedRigidbodies = new List<Rigidbody>();

    // 用於儲存機關是否已啟動的狀態
    private bool isActivated = true;

    void Start()
    {
        // 根據設定決定初始是否啟動移動
        isActivated = autoStart;

        // 計時器減去延遲時間
        timer = -startDelay;

        // 安全防護：如果忘記設定點，預設目前位置為唯一起點
        if (localPoints == null || localPoints.Length == 0)
        {
            localPoints = new Vector3[] { transform.localPosition };
        }

        // 遊戲開始時，直接傳送到第一個路徑點
        transform.localPosition = localPoints[0];
        targetIndex = 0;
    }

    void Update()
    {
        // 如果機關未啟動，則不進行移動
        if (!isActivated) return;

        // 計時累加，若還在延遲時間內則先不移動
        timer += Time.deltaTime;
        if (timer < 0) return;

        // 如果已經完成單次移動，或者點的數量太少，則不移動
        if (isFinished || localPoints.Length <= 1) return;

        Vector3 targetLocalPos = localPoints[targetIndex];
        // 記錄移動前的位置 (世界座標)
        Vector3 posBeforeMove = transform.position;

        // 進行本機座標的平滑移動
        transform.localPosition = Vector3.MoveTowards(
            transform.localPosition, 
            targetLocalPos, 
            speed * Time.deltaTime
        );

        // 計算這一幀平台產生的位移差 (世界座標)
        Vector3 deltaMove = transform.position - posBeforeMove;

        // 若開啟載物功能，將位移差套用給所有平台上的物件
        if (carriesObjects)
        {
            // 清理已銷毀的物件，避免產生 NullReferenceException
            carriedRigidbodies.RemoveAll(rb => rb == null);

            // 套用位移
            foreach (Rigidbody rb in carriedRigidbodies)
            {
                rb.position += deltaMove;
            }
        }

        // 判斷是否非常接近目標點
        if (Vector3.Distance(transform.localPosition, targetLocalPos) < 0.05f)
        {
            // 更新下一個目標點
            UpdateTargetIndex();
        }
    }

    private void UpdateTargetIndex()
    {
        if (movementType == MoveType.Once)
        {
            if (targetIndex < localPoints.Length - 1)
            {
                targetIndex++;
            }
            else
            {
                isFinished = true; // 到達最後一點，停止移動
            }
        }
        else if (movementType == MoveType.Loop)
        {
            targetIndex = (targetIndex + 1) % localPoints.Length;
        }
        else if (movementType == MoveType.PingPong)
        {
            if (movingForward)
            {
                if (targetIndex < localPoints.Length - 1)
                {
                    targetIndex++;
                }
                else
                {
                    movingForward = false;
                    targetIndex--;
                }
            }
            else
            {
                if (targetIndex > 0)
                {
                    targetIndex--;
                }
                else
                {
                    movingForward = true;
                    targetIndex++;
                }
            }
        }
    }

    // 檢查物件標籤是否在允許的清單內
    private bool IsTargetTag(string tag)
    {
        if (carriedTags == null || carriedTags.Length == 0) return false;
        foreach (string t in carriedTags)
        {
            if (t == tag) return true;
        }
        return false;
    }

    // 偵測物件踩上平台
    private void OnCollisionEnter(Collision collision)
    {
        if (carriesObjects && IsTargetTag(collision.gameObject.tag))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null && !carriedRigidbodies.Contains(rb))
            {
                carriedRigidbodies.Add(rb);
            }
        }
    }

    // 偵測物件離開平台
    private void OnCollisionExit(Collision collision)
    {
        if (carriesObjects && IsTargetTag(collision.gameObject.tag))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null && carriedRigidbodies.Contains(rb))
            {
                carriedRigidbodies.Remove(rb);
            }
        }
    }

    // 在編輯器場景中繪製路徑輔助線
    private void OnDrawGizmos()
    {
        Transform parentTransform = transform.parent;

        if (localPoints == null || localPoints.Length == 0) return;

        Gizmos.color = pathColor;
        Vector3[] worldPoints = new Vector3[localPoints.Length];

        // 計算每個路徑點的世界座標
        for (int i = 0; i < localPoints.Length; i++)
        {
            worldPoints[i] = parentTransform != null ? parentTransform.TransformPoint(localPoints[i]) : localPoints[i];
            Gizmos.DrawSphere(worldPoints[i], 0.15f);
        }

        // 繪製路徑連接線
        for (int i = 0; i < worldPoints.Length - 1; i++)
        {
            Gizmos.DrawLine(worldPoints[i], worldPoints[i + 1]);
        }

        // 如果是循環模式，將最後一點與起點連線
        if (movementType == MoveType.Loop && worldPoints.Length > 1)
        {
            Gizmos.DrawLine(worldPoints[worldPoints.Length - 1], worldPoints[0]);
        }
    }

    // ------------------------------------------
    // 以下為供 TriggerSwitch 外部呼叫的控制方法
    // ------------------------------------------

    public void Activate()
    {
        isActivated = true;
    }

    public void Deactivate()
    {
        isActivated = false;
    }

    public void ToggleActivate()
    {
        isActivated = !isActivated;
    }
}
