using UnityEngine;

public class RotatingObstacle : MonoBehaviour
{
    [Header("旋轉設定")]
    [Tooltip("物件自轉的速度 (度/秒)。可設定 X, Y, Z 三個方向的自轉速度。")]
    public Vector3 rotateSpeed = new Vector3(0f, 90f, 0f);

    void Update()
    {
        // 每幀根據時間差進行旋轉
        transform.Rotate(rotateSpeed * Time.deltaTime);
    }
}
