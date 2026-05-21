using UnityEngine;

public class Pendulum : MonoBehaviour
{
    public enum RotationAxis
    {
        X,
        Y,
        Z
    }

    [Header("擺動設定")]
    [Tooltip("最大來回擺動的角度 (例如 45 代表在左右 45 度之間擺動)")]
    public float limitAngle = 45f;

    [Tooltip("擺動速度 (數值越大擺得越快)")]
    public float speed = 2f;

    [Tooltip("相位/時間偏移 (秒)。可用於讓同一排的多個擺錘產生交錯擺動的時間差")]
    public float timeOffset = 0f;

    [Tooltip("來回擺動旋轉的軸向，通常是 X 軸或 Z 軸")]
    public RotationAxis swingAxis = RotationAxis.Z;

    private Quaternion startRotation;

    void Start()
    {
        // 記錄起使旋轉角度，以防在場景中手動旋轉擺錘的朝向後擺動方向跑掉
        startRotation = transform.localRotation;
    }

    void Update()
    {
        // 使用 Sin 函數計算出隨時間來回波動的角度值 (-limitAngle ~ +limitAngle)
        float angle = limitAngle * Mathf.Sin((Time.time + timeOffset) * speed);

        // 根據選定的軸向計算相對旋轉 Quaternion
        Quaternion localOffsetRotation = Quaternion.identity;
        switch (swingAxis)
        {
            case RotationAxis.X:
                localOffsetRotation = Quaternion.AngleAxis(angle, Vector3.right);
                break;
            case RotationAxis.Y:
                localOffsetRotation = Quaternion.AngleAxis(angle, Vector3.up);
                break;
            case RotationAxis.Z:
                localOffsetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                break;
        }

        // 以起始旋轉為基底，疊加擺動角度
        transform.localRotation = startRotation * localOffsetRotation;
    }
}
