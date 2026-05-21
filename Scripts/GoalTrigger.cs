using UnityEngine;
using UnityEngine.UI; // 引入 Legacy UI 命名空間
using TMPro; // 引入 TextMeshPro 命名空間
using UnityEngine.SceneManagement; // 引入場景管理命名空間
using System.Collections;

public class GoalTrigger : MonoBehaviour
{
    public enum TextType
    {
        TextMeshPro,
        LegacyText
    }

    [Header("UI 類型設定")]
    [Tooltip("選擇您在場景中使用的 UI 文字元件類型")]
    public TextType textType = TextType.TextMeshPro;

    // 以下兩個欄位由自訂編輯器 (GoalTriggerEditor) 根據下拉選單選擇動態呈現
    [HideInInspector]
    public TextMeshProUGUI winTextTMP;

    [HideInInspector]
    public Text winTextLegacy;

    [Tooltip("通關時要顯示的文字內容")]
    public string winMessage = "恭喜通關！🎉";

    [Header("關卡切換設定")]
    [Tooltip("觸發終點後，是否自動載入下一個關卡場景")]
    public bool loadNextLevel = true;

    [Tooltip("下一關的場景名稱。如果留空，將會自動載入 Build Settings 中排列的下一個場景")]
    public string nextSceneName = "";

    [Tooltip("顯示通關文字後，延遲多久才載入下一關 (秒)")]
    public float loadDelay = 2.0f;

    [Header("玩家設定")]
    [Tooltip("玩家的 Tag 標籤，預設為 Player")]
    public string playerTag = "Player";

    private bool isTriggered = false;

    void Start()
    {
        // 遊戲開始時，先將所選的文字元件清空
        SetText("");
    }

    // 當有物件進入 Trigger 觸發器時執行
    private void OnTriggerEnter(Collider other)
    {
        // 檢查進入的物件是否為玩家，且尚未被觸發
        if (other.CompareTag(playerTag) && !isTriggered)
        {
            isTriggered = true;
            SetText(winMessage);

            if (loadNextLevel)
            {
                StartCoroutine(LoadNextLevelSequence());
            }
        }
    }

    private IEnumerator LoadNextLevelSequence()
    {
        // 等待指定時間讓玩家看清通關文字
        yield return new WaitForSeconds(loadDelay);

        // 如果手動指定了場景名稱，則載入該場景
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            // 否則自動載入 Build Settings 的下一個場景
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            
            // 檢查下一個 Index 是否超出 Build Settings 中設定的場景總數
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                Debug.LogWarning("GoalTrigger: 已經是 Build Settings 中的最後一關，無法載入下一關！");
            }
        }
    }

    // 統一更新文字的方法
    private void SetText(string text)
    {
        if (textType == TextType.TextMeshPro && winTextTMP != null)
        {
            winTextTMP.text = text;
        }
        else if (textType == TextType.LegacyText && winTextLegacy != null)
        {
            winTextLegacy.text = text;
        }
    }
}
