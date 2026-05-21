using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("移動物理設定")]
    [Tooltip("移動施加的力道")]
    public float moveForce = 10f;

    [Header("Input Action 輸入設定")]
    [Tooltip("拖入外部設定好的 InputActionReference 資源 (優先選用)")]
    public InputActionReference moveActionReference;

    [Tooltip("手動設定面板 (當沒有拖入外部 Reference 時，可在下方手動綁定)")]
    public InputAction moveAction;

    private Rigidbody rb;
    private InputAction activeMoveAction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // 判斷按鍵綁定的來源優先權
        if (moveActionReference != null)
        {
            activeMoveAction = moveActionReference.action;
        }
        else if (moveAction != null && moveAction.bindings.Count > 0)
        {
            activeMoveAction = moveAction;
        }
        else
        {
            // 當兩者皆空時，程式進行防呆保護，自動建立預設鍵盤 WASD/方向鍵/手把搖桿綁定
            if (moveAction == null)
            {
                moveAction = new InputAction("Move", type: InputActionType.Value);
            }

            // 綁定 1: WASD
            moveAction.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/w")
                .With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/a")
                .With("Right", "<Keyboard>/d");

            // 綁定 2: 鍵盤方向鍵
            moveAction.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/upArrow")
                .With("Down", "<Keyboard>/downArrow")
                .With("Left", "<Keyboard>/leftArrow")
                .With("Right", "<Keyboard>/rightArrow");

            // 綁定 3: 手把左類比搖桿
            moveAction.AddBinding("<Gamepad>/leftStick");

            activeMoveAction = moveAction;
        }
    }

    private void OnEnable()
    {
        if (activeMoveAction != null)
        {
            activeMoveAction.Enable();
        }
    }

    private void OnDisable()
    {
        if (activeMoveAction != null)
        {
            activeMoveAction.Disable();
        }
    }

    void FixedUpdate()
    {
        if (activeMoveAction == null) return;

        // 讀取目前的 2D 移動輸入值
        Vector2 input = activeMoveAction.ReadValue<Vector2>();
        Vector3 forceDirection = new Vector3(input.x, 0f, input.y);

        // 進行斜向移動速度正規化，避免斜走速度加倍的問題
        if (forceDirection.magnitude > 1f)
        {
            forceDirection.Normalize();
        }

        // 對剛體施加物理力
        rb.AddForce(forceDirection * moveForce);
    }
}