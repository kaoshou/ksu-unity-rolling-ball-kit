# 🎮 Unity 3D 滾球遊戲關卡設計工具箱 (Unity 3D Rolling Ball Level Design Toolbox)

> 🏫 **本程式碼與工具箱是 崑山科技大學 鄭郁翰老師 用於「遊戲程式設計」中入門單元課程的教學教具與專案範本。**

本工具箱專為 **Unity 遊戲開發教學與快速關卡原型設計** 打造。內含 15 個高度精簡、邏輯防呆且參數完全公開的 C# 互動腳本。學生與開發者只需拖放腳本、在 Inspector 調整數值，即可在幾分鐘內建構出具有高度遊戲性與物理互動的滾球關卡（低程式碼/無程式碼設計）。

---

## 📖 給新手的 Unity 基礎觀念小學堂

如果您是第一次接觸 Unity 的新手，在開始動手前，花兩分鐘理解以下四個超基本概念，可以讓您的學習與操作順暢十倍！

### 1. 🖥️ 認得四大常用面板
*   **Hierarchy (階層面板)**：顯示目前場景中「所有存在的物件」清單。在這裡建立、命名你的球體與地板。
*   **Scene (場景視窗)**：你的 3D 編輯畫布。可以用滑鼠在這裡拖拉移動、旋轉或縮放你的物件。
*   **Project (專案面板)**：像電腦的資料夾，存放你「所有的檔案」（如 C# 腳本、材質、音效、關卡場景）。**物件要掛載的腳本，都是從這裡拖出去的**。
*   **Inspector (屬性檢視面板)**：當你點選場景中的某個物件時，右側會顯示它身上的所有「組件 (Component)」與設定數值（例如物體的重量、摩擦力、腳本參數等）。

### 2. 🪨 「剛體 (Rigidbody)」與「碰撞器 (Collider)」的差別
*   **Collider (碰撞器)** = **物體的肉體（物理邊界）**。
    *   沒有它，物體就只是幽靈，任何東西都會直接穿透過去。
    *   *地板只需要 Collider，球體也需要 Collider。*
*   **Rigidbody (剛體)** = **物理規律的靈魂（重力與受力）**。
    *   有它，物體才會受到地球重力影響往下掉，被撞到時才會飛出去，並且能被施加推力滾動。
    *   *只有「會動、會滾、會掉落」的玩家球體或物理木箱才需要掛載 Rigidbody！固定不動的地面、尖刺障礙則千萬不要加 Rigidbody。*

### 3. 🎨 如何將 C# 腳本掛載到物件上？（兩種常用方式）
*   **方式一（最直觀）**：直接把腳本從 **Project (專案面板)** 拖曳，放到 **Hierarchy (階層面板)** 的目標物件上方放開。
*   **方式二（最精準）**：點選目標物件，在右側 **Inspector 面板** 最下方點擊 **`Add Component`** 按鈕，輸入腳本名稱（例如 `PlayerController`）並按 Enter 新增。

### 4. 🌈 如何幫物件塗上漂亮的顏色？（材質 Material）
預設建立的 3D 物件都是單調的灰色，你可以透過以下步驟幫關卡上色：
1. 在 **Project 面板** 的空白處點擊滑鼠右鍵 -> **Create -> Material**。
2. 將新產生的材質球重新命名（例如 `Red_Danger`）。
3. 點選該材質球，在右側 **Inspector 面板** 找到 **`Surface Inputs`** 下方的 **`Base Map`**（舊版 Unity 稱為 **`Albedo`**），點擊旁邊的白色區塊，即可選擇你想要的顏色。
4. **套用顏色**：直接將這顆材質球從 Project 面板拖曳到 **Scene 視窗** 中你要上色的物件（例如球體或牆壁）上方，物件就會立刻變色！

---

## 🚀 快速開始：5分鐘建構基礎關卡

跟著以下步驟，在一個全新的 Unity 6.3 場景中建立您的第一個滾球關卡：

### 步驟 0：在 Unity Hub 建立 Universal 3D 專案
1. 打開 **Unity Hub** 軟體。
2. 點擊右上角的 **`新專案`** (New Project) 按鈕。
3. 在視窗上方的 **編輯器版本** 下拉選單中，確認選擇了 **`6000.3.x LTS`** (即 Unity 6 版本)。
4. 在中間的專案範本清單中點選 **`Universal 3D`**（本課程工具箱以 Universal 3D (URP) 範本為原則進行開發，能提供高精緻度的渲染與光影效果）。
5. 在右側設定面板中：
   * **專案名稱**：輸入您的專案名稱（例如：`My project (1)`）。
   * **位置**：選擇您的儲存路徑（例如：`D:\Work`，建議選擇全英文路徑）。
6. 點擊右下角的 **`建立專案`** 按鈕，靜待 Unity 6 自動建置專案並開啟主編輯器。
7. 編輯器開啟後，在下方的 **Project (專案面板)** 中找到 `Assets` 資料夾，點選右鍵 -> **Create -> Folder**，建立一個名為 **`Scripts`** 的新資料夾。將本工具箱的所有 C# 腳本（連同 `Editor` 子資料夾）直接拖入此資料夾內，即可開始建置關卡！

### 步驟 1：建立地板（關卡地面）
1. 在 Hierarchy (階層) 視窗點擊滑鼠右鍵，選擇 **3D Object -> Cube**（或 Plane）。
2. 將其重新命名為 `Floor`。
3. 在 Inspector 視窗中，將其 **Scale (縮放)** 設為 `(20, 1, 20)`，以建立一個寬敞的平台。
4. 確保其身上帶有 **Box Collider** 且 **`Is Trigger` 未勾選**（以提供實體物理碰撞，防止球掉下去）。

### 步驟 2：建立玩家球體與操作
1. 在 Hierarchy 視窗點擊右鍵，選擇 **3D Object -> Sphere**。
2. 將其重新命名為 `Player`。
3. 將其 **Position (座標)** 設為 `(0, 2, 0)`（使其懸空在地板上方）。
4. 在 Inspector 點擊 **Add Component**，搜尋並加入 **Rigidbody** (剛體組件)。
5. **【最關鍵步驟】**：在 Inspector 最上方，將其 **Tag (標籤)** 下拉選單設為 **`Player`**（場景中幾乎所有機關都依賴此 Tag 來辨識玩家）。
6. 點擊 **Add Component**，搜尋並掛載 **`PlayerController.cs`**。
7. *提示*：本腳本相容 Input System。若無特別設定，遊戲開始時會自動產生預設按鍵（鍵盤 `WASD`、`方向鍵`、以及手把`左類比搖桿`）。

---

## 🎥 相機跟隨設定（二選一）

為了讓遊戲畫面一直跟著滾動的球體，您必須設定相機跟隨。這裡提供兩種適合滾球遊戲的配置方式：

### 方式 A：使用自訂極簡腳本 `CameraFollow.cs`（適合新手教學）
本方式完全不需要安裝額外套件，適合初學者快速上手：
1. 在 Hierarchy 視窗點選 **Main Camera**。
2. 點擊 **Add Component**，搜尋並掛載 **`CameraFollow.cs`**。
3. 將 Hierarchy 中的 **`Player`** 物件直接拖入腳本的 **`Target`** 欄位中（若忘記拖入，腳本在遊戲執行時會自動尋找 Tag 為 `Player` 的物件進行防呆綁定）。
4. **設定偏移值 (Offset)**：在面板上設定相機與球體的距離（建議值為 X: `0`, Y: `6`, Z: `-8`）。
5. *提示*：如果在 Inspector 將 `Offset` 設為 `(0, 0, 0)`，腳本會在遊戲啟動時自動讀取場景中相機與球體的「初始相對距離」作為偏移值。

---

### 方式 B：使用 `Cinemachine` 系統（適合高質感與競速遊戲）
Cinemachine 是 Unity 官方強大的相機系統，能提供極度平滑的跟隨、防穿牆判定與運鏡效果。

#### 1. 安裝 Cinemachine
* 在 Unity 頂部選單選擇 **Window -> Package Manager**。
* 將左上角選單切換至 **Packages: Unity Registry**。
* 搜尋 **Cinemachine**，並點擊 **Install** 安裝。

#### 2. 建立虛擬相機
* 在 Hierarchy 點擊右鍵，選擇 **Cinemachine -> Virtual Camera**（新版 Unity 6 中為 **Create -> Camera -> Virtual Camera**）。
* 這會在場景中建立一個名為 `Virtual Camera` 的物件，同時 Main Camera 身上會被自動掛載一個 `Cinemachine Brain` 組件。

#### 3. 設定追蹤目標（防打滑/旋轉配置）
* 點選 `Virtual Camera` 物件。
* 將 Hierarchy 中的 **`Player`** 物件拖入其 **`Follow` (跟隨)** 與 **`Look At` (看向)** 欄位。
* **【滾球遊戲專屬設定】**：因為玩家的球體在滾動時會**高速自轉**，若使用預設的相機設定，畫面會跟著球體瘋狂旋轉導致頭暈。請務必進行以下設定：
  * 在下方 **Body** 屬性中，將類型改為 **`Transposer`**。
  * 將 **Binding Mode** 設為 **`Simple Follow With World Up`**（這會讓相機只追蹤球的位置，並維持世界座標的朝上方向，完全忽視球體的自轉）。
  * 設定 **Follow Offset (跟隨偏移)**：建議設為 `(0, 6, -8)`。
  * 在下方 **Aim** 屬性中，將類型改為 **`Same As Follow Target`** 或 **`Composer`**。如果使用 **Composer**，可以微調 **Tracked Object Offset** 的 Y 軸（例如設為 `1`），讓視角稍微看向上方。
  * 微調 **Damping (阻尼)**：將 Body 與 Aim 下的 X, Y, Z Damping 設在 `0.1 ~ 0.5` 之間，即可獲得非常流暢、具有物理慣性的相機跟隨效果。

#### 4. 啟用防穿牆遮擋（Cinemachine Collider）
* 在 `Virtual Camera` 物件的 Inspector 最下方，點選 **Add Extension** 下拉選單。
* 選擇 **Cinemachine Collider**。
* 這會使相機在被牆壁阻擋時自動拉近，絕對不會穿透到地板或障礙物下方，提供最專業的 3D 遊戲視角。

---

## 🗂️ 腳本清單與快速跳轉

點選下方連結可快速跳至該腳本的詳細設定與前置作業說明：

| 編號 | 腳本名稱 | 核心用途簡述 | 快速連結 |
| :--- | :--- | :--- | :--- |
| 1 | `PlayerController` | 控制球體的滾動與方向輸入。 | [跳至詳細說明](#1-playercontrollercs) |
| 2 | `CameraFollow` | 極簡硬跟隨相機，防止旋轉打滑。 | [跳至詳細說明](#2-camerafollowcs) |
| 3 | `FallDetector` | 墜落高度偵測器，支援記錄點傳送或整關重來。 | [跳至詳細說明](#3-falldetectorcs) |
| 4 | `PathMovement` | 多功能路徑移動，可做為移動平台或移動障礙。 | [跳至詳細說明](#4-pathmovementcs) |
| 5 | `RotatingObstacle` | 控制物件規律自轉（如旋轉風扇、滾石）。 | [跳至詳細說明](#5-rotatingobstaclecs) |
| 6 | `GoalTrigger` | 通關終點，顯示通關 UI 並自動載入下一關。 | [跳至詳細說明](#6-goaltriggercs) |
| 7 | `SpeedBoostItem` | 瞬間向前噴射衝刺的加速板。 | [跳至詳細說明](#7-speedboostitemcs) |
| 8 | `JumpPad` | 垂直高高彈起的跳躍板。 | [跳至詳細說明](#8-jumppadcs) |
| 9 | `Hazard` | 傷害機關（尖刺/岩漿），碰觸後重置或傳送。 | [跳至詳細說明](#9-hazardcs) |
| 10 | `TriggerSwitch` | 開關按鈕，碰觸後啟動/隱藏物件或控制平台移動。 | [跳至詳細說明](#10-triggerswitchcs) |
| 11 | `Teleporter` | 傳送門，兩點對傳並內建防無限循環冷卻。 | [跳至詳細說明](#11-teleportercs) |
| 12 | `CrumblingPlatform` | 踩上後會微震動並塌陷消失的易碎地板。 | [跳至詳細說明](#12-crumblingplatformcs) |
| 13 | `Pendulum` | 規律來回晃動的鐘擺（如巨斧、重錘）。 | [跳至詳細說明](#13-pendulumcs) |
| 14 | `Checkpoint` | 記錄點，觸發後變色，並更新玩家的復活存檔點。 | [跳至詳細說明](#14-checkpointcs) |
| 15 | `MoveForceModifier` | 狀態藥水，暫時或永久改變玩家的移動操控推力。 | [跳至詳細說明](#15-moveforcemodifiercs) |

---

## 📖 腳本詳細使用說明

---

### 1. `PlayerController.cs` (玩家球體控制器)
*   **用途**：掛載在主角球體上，負責接收玩家輸入（鍵盤/手把），對球體的剛體（Rigidbody）施加物理力（AddForce）來驅動滾動。
*   **前置作業**：
    *   目標物件必須有 **Rigidbody** (剛體) 組件。
    *   目標物件必須有 **Collider** (球體碰撞器，如 Sphere Collider)，且 **`Is Trigger` 必須取消勾選**。
*   **Inspector 屬性詳細說明**：
    *   `moveForce` (力道大小，預設 `10.0`)：每次移動輸入施加力道大小。數值越高，球體滾動的加速度與最高速度越快。
    *   `moveActionReference` (InputAction 資源關聯)：可選，拖入專案中建立的 InputActionReference 資源。
    *   `moveAction` (手動按鍵輸入設定)：若無拖入外部 Reference，可在此展開手動綁定鍵盤/手把控制。
    *   *註：若以上兩者皆留空，程式會於啟動時自動為學生建立預設 WASD、方向鍵、以及手把類比搖桿的防呆控制設定。*

---

### 2. `CameraFollow.cs` (相機追蹤控制器)
*   **用途**：掛載在主相機上，追蹤球體的移動。使用直接的世界座標位移計算，完全避免了相機跟隨打滑旋轉的現象。
*   **前置作業**：
    *   通常掛載在 **Main Camera** 上。
    *   場景中必須有標籤為 **`Player`** 的主角球體。
*   **Inspector 屬性詳細說明**：
    *   `target` (跟隨目標，預設 `Null`)：欲追蹤的球體 Transform。若保持留空，遊戲啟動時會自動尋找 Tag 為 `Player` 的物件進行綁定。
    *   `offset` (相對座標偏移量，預設 `(0, 0, 0)`)：相機與球體之間的 X, Y, Z 三軸相對距離。
    *   *註：如果 `offset` 設為 `(0, 0, 0)`，則程式會自動在 Start 時讀取「當前編輯器中相機與球體的距離」做為跟隨偏移。*

---

### 3. `FallDetector.cs` (墜落高度偵測器)
*   **用途**：監測玩家球體高度。一旦球體掉落至指定高度界線下方，執行重新載入關卡或傳送復活。
*   **前置作業**：
    *   掛載在帶有 **Rigidbody** 且 Tag 為 `Player` 的玩家球體上。
*   **Inspector 屬性詳細說明**：
    *   `fallLimitY` (死亡臨界 Y 軸高度，預設 `-10.0`)：當玩家球體的世界 Y 座標低於此值時，判定為墜落死亡。
    *   `reloadScene` (是否重新載入關卡，預設 `true`)：
        *   若為 `true`：立刻重新載入當前 Scene（所有機關與狀態都會重設）。
        *   若為 `false`：僅執行座標傳送。會優先傳送到最新踩到的 `Checkpoint` 記錄點；若無踩過記錄點，則傳送到 `respawnPoint`。
    *   `respawnPoint` (起始復活點 Transform，預設 `Null`)：當 `reloadScene` 設為 `false` 且玩家「未踩過任何記錄點」時，球體墜落後會被傳送回此處。

---

### 4. `PathMovement.cs` (多功能路徑移動平台/障礙物)
*   **用途**：控制平台或障礙物在多個座標點之間來回移動。本腳本自適應 local 本機座標，可直接作為「載人平台（會帶動玩家移動）」或「移動撞牆障礙物」。
*   **前置作業**：
    *   物件本身必須帶有 **Collider**。若作為平台供站立，**`Is Trigger` 必須取消勾選**。
*   **3D 編輯器手把與快捷面板詳細說明**：
    *   **3D Handles**：選取此物件時，Scene 視窗會直接繪製出所有設定好的路徑點，每個路徑點上方都有一個紅藍綠 3D 移動箭頭，直接用滑鼠拖拉箭頭即可調整路徑點，不用手動打字輸入座標。
    *   `將物件『當前位置』新增為新路徑點` 按鈕：學生可以用滑鼠將平台拖到想去的位置，按一下按鈕即可直接在陣列最尾端新增此位置，設計多點路徑極度快速。
    *   `重設所有路徑點 (只保留目前位置為唯一點)` 按鈕：一鍵重設，防呆清空。
*   **Inspector 屬性詳細說明**：
    *   `movementType` (移動模式，預設 `PingPong`)：
        *   `Loop`：循環式（A -> B -> C -> 起點 A -> B...）。
        *   `PingPong`：折返式（A -> B -> C -> B -> A...）。
        *   `Once`：單次式（A -> B -> C 後就停在最後一點）。
    *   `localPoints` (本機路徑點陣列，Vector3 陣列)：相對於父物件（Parent）的局部座標路徑點。
    *   `speed` (移動速度，預設 `2.0`)：平台移動的速度。
    *   `startDelay` (啟動延遲時間，預設 `0.0`)：遊戲啟動後要等待幾秒才開始移動，適合用來控制一排障礙物錯開時間啟動。
    *   `carriesObjects` (載人/載物設定，預設 `true`)：
        *   若為 `true`：當玩家（或有 Rigidbody 的箱子）踩在平台上方時，會自動隨平台同歩位移，不會因為慣性從移動平台滑落（做為「移動平台/地板」使用）。
        *   若為 `false`：物件碰觸平台不會跟著移（做為「移動障礙物/推人牆面」使用）。
    *   `carriedTags` (可載運物件標籤清單，預設 `Player`)：只有帶有此清單內標籤的物件，才會在踩上時啟用載動效果。
    *   `autoStart` (自動啟動，預設 `true`)：是否在遊戲開始時立刻移動。若取消勾選，平台預設不動，需要透過 `TriggerSwitch` (開關) 觸發遙控。
    *   `pathColor` (輔助線顏色，預設 `Cyan 青藍色`)：在 Scene 視窗中繪製路徑點球體與路徑線的預覽顏色。

---

### 5. `RotatingObstacle.cs` (規律自轉障礙物)
*   **用途**：控制物件進行持續性的自轉，適合製作旋轉風扇、轉動滾筒、尖刺障礙等。
*   **前置作業**：
    *   物件本身有 **Collider** 可與球體碰撞。
*   **Inspector 屬性詳細說明**：
    *   `rotateSpeed` (旋轉速度向量，預設 `(0, 50, 0)`)：物件繞著局部座標軸自轉的速度（單位：度/秒）。修改 Y 軸數值可以控制水平旋轉，修改 X 或 Z 軸可以控制翻轉。

---

### 6. `GoalTrigger.cs` (通關終點觸發器)
*   **用途**：當玩家滾入終點區域，更新畫面通關字樣 UI，並於延遲時間後執行新關卡轉場載入。
*   **前置作業**：
    *   終點物件的 Collider **必須勾選 `Is Trigger`**。
*   **Inspector 屬性詳細說明**：
    *   `textType` (UI 文字類型，預設 `TextMeshPro`)：選擇您在場景中使用的是新版 `TextMeshPro` 還是舊版 `LegacyText` 元件（面板會根據您的選擇自動隱藏與顯示對應的拖曳欄位）。
    *   `winTextTMP` (TextMeshProUGUI 文字元件)：拖入畫面上建立的 TextMeshProUGUI。
    *   `winTextLegacy` (Legacy Text 文字元件)：拖入畫面上建立的 Legacy Text 物件。
    *   `winMessage` (通關文字內容，預設 `恭喜通關！🎉`)：玩家踩到終點時要顯示在文字元件上的字串。
    *   `loadNextLevel` (載入下一關，預設 `true`)：是否在抵達終點後執行關卡自動轉場。
    *   `nextSceneName` (指定下一關場景名稱，預設留空)：要載入的場景英文名稱。**保持留空時，系統會自動按 Build Settings 排列順序載入下一個場景**，極力避免手動打字產生的拼寫 Bug。
    *   `loadDelay` (載入延遲秒數，預設 `2.0`)：顯示通關文字到開始轉場之間的時間差，讓玩家有時間慶祝。
    *   `playerTag` (識別玩家的標籤，預設 `Player`)：限定只有帶有此標籤的物件可以觸發終點。

---

### 7. `SpeedBoostItem.cs` (加速衝刺道具)
*   **用途**：加速踏板或加速藥水。碰觸後使玩家獲得瞬間噴射推力，隨後道具暫時消失，冷卻後重新出現。
*   **前置作業**：
    *   物件的 Collider **必須勾選 `Is Trigger`**。
*   **Inspector 屬性詳細說明**：
    *   `boostForce` (衝刺推力，預設 `20.0`)：碰觸時對玩家施加的向前推力大小。
    *   `cooldownDuration` (重生冷卻時間，預設 `3.0`)：被吃掉後到再次出現的秒數。
    *   `playerTag` (玩家的標籤，預設 `Player`)。

---

### 8. `JumpPad.cs` (彈跳跳躍板)
*   **用途**：當玩家碰觸此物時，會被高高垂直向上彈射（類似彈跳床）。
*   **前置作業**：
    *   物件的 Collider **必須勾選 `Is Trigger`**，以保證順暢穿透觸發。
*   **Inspector 屬性詳細說明**：
    *   `jumpForce` (彈跳推力，預設 `15.0`)：給予玩家垂直向上（Y 軸）的物理力大小。
    *   `playerTag` (玩家的標籤，預設 `Player`)。

---

### 9. `Hazard.cs` (傷害/危險陷阱機關)
*   **用途**：掛載在危險物（如岩漿、地刺、致命雷射、滾石）。當球碰到後，重置或傳送。
*   **前置作業**：
    *   **雙重偵測**：Collider 勾選或不勾選 `Is Trigger` 皆可。實體碰撞障礙物與隱形死亡區域皆能觸發判定。
*   **Inspector 屬性詳細說明**：
    *   `reloadScene` (是否重新載入關卡，預設 `true`)：
        *   若為 `true`：直接重新載入整個關卡。
        *   若為 `false`：執行座標傳送。會優先傳送到最新 `Checkpoint` 記錄點，其次傳送回 `respawnPoint`。
    *   `respawnPoint` (起始復活點 Transform，預設 `Null`)：無踩過記錄點時的備用傳送位置。
    *   `playerTag` (玩家的標籤，預設 `Player`)。

---

### 10. `TriggerSwitch.cs` (開關遙控按鈕)
*   **用途**：開關/壓板。可用來遙控開啟另一扇門（顯示/隱藏）或控制平台的移動狀態。
*   **前置作業**：
    *   Collider 勾選或不勾選 `Is Trigger` 皆可（可作為實體按鈕或隱形踩踏區域）。
    *   屬性面板為動態排版，會根據「觸發行為」之設定，動態隱藏與顯示對應所需關聯之欄位。
*   **Inspector 屬性詳細說明**：
    *   `triggerAction` (觸發行為，預設 `ToggleShowHide`)：選擇開關控制的行為：
        *   `ShowObject`：顯示（啟動）目標遊戲物件。
        *   `HideObject`：隱藏（關閉）目標遊戲物件。
        *   `ToggleShowHide`：切換目標物件的顯示/隱藏狀態（顯示變隱藏，隱藏變顯示）。
        *   `StartMovement`：讓目標平台（PathMovement）開始移動。
        *   `StopMovement`：讓目標平台停止移動。
        *   `ToggleMovement`：切換目標平台的移動/停止狀態。
    *   `targetObject` (控制目標物件，預設 `Null`)：當 `triggerAction` 設為顯示/隱藏相關時會顯示此拖曳欄位，用來拖入被控制的物體（如門、隱形橋梁）。
    *   `targetPlatform` (控制移動平台，預設 `Null`)：當 `triggerAction` 設為控制平台移動相關時會顯示此拖曳欄位，用來拖入掛載了 `PathMovement` 的平台。
    *   `triggerOnce` (只觸發一次，預設 `true`)：是否為一次性開關（如收集鑰匙開門）。若為 `false`，則可來回重複踩踏觸發。
    *   `cooldown` (防重複觸發冷卻時間，預設 `1.0` 秒)：非一次性開關時，每次觸發後需要間隔幾秒才能再次被觸發。
    *   `changeColorOnTrigger` (觸發時改變顏色，預設 `true`)：踩下時開關本體是否變色以提供視覺回饋。
    *   `triggeredColor` (觸發後的顏色，預設 `Green 綠色`)：開關被觸發後，開關自身的 MeshRenderer 會變成此種顏色。
    *   `destroyAfterTrigger` (觸發後本身消失，預設 `false`)：勾選後，一旦開關被觸發，自己會立刻銷毀消失（非常適合用來製作一次性吃掉的「關卡門鑰匙」道具）。
    *   `playerTag` (玩家的標籤，預設 `Player`)。

---

### 11. `Teleporter.cs` (傳送門)
*   **用途**：成對的傳送點。玩家踩上 A 門後，會被傳送至 B 門位置，內建防互傳死循環判定。
*   **前置作業**：
    *   傳送門物件的 Collider **必須勾選 `Is Trigger`**。
*   **Inspector 屬性詳細說明**：
    *   `destination` (傳送目的地，預設 `Null`)：要傳送去的位置（通常是拖入另一個傳送門的 Transform）。
    *   `keepVelocity` (保留速度，預設 `true`)：若為 `true`，傳送後球體會繼承原本的加速度與滾動方向；若為 `false`，傳送後球體會靜止在目的地。
    *   `playerTag` (玩家的標籤，預設 `Player`)。

---

### 12. `CrumblingPlatform.cs` (易碎崩塌地板)
*   **用途**：踩上後會微震動，並在一段時間後塌陷消失，適合限制玩家停留時間。
*   **前置作業**：
    *   平台物件的 Collider **絕對不能勾選 `Is Trigger`**，以便玩家可以站在上面。
*   **Inspector 屬性詳細說明**：
    *   `crumbleDelay` (塌陷時間延遲，預設 `1.5` 秒)：從玩家第一次踩上地板，到地板徹底消失之間的時間長度。
    *   `respawnDelay` (重生冷卻時間，預設 `3.0` 秒)：地板塌陷消失後，隔多少秒會原處重新浮現。
    *   `shakeAmount` (震動幅度，預設 `0.05` 公尺)：塌陷前震動警告的劇烈搖晃位移幅度。
    *   `playerTag` (玩家的標籤，預設 `Player`)。

---

### 13. `Pendulum.cs` (來回晃動擺錘)
*   **用途**：繞著頂部中心軸，以簡諧運動來回規律晃動的鐘擺（如吊著的大鐵球）。
*   **前置作業**：
    *   擺錘模型在 3D 軟體中的中心點 (Pivot) 必須設定在**模型最頂端的懸掛點（擺動圓心）**，否則擺動軌跡會打轉。
    *   若想讓球被擺錘打飛，需要在擺錘本體掛載 **Rigidbody (將 Is Kinematic 勾選)** 與 **Collider**，並掛載 `Hazard` 腳本。
*   **Inspector 屬性詳細說明**：
    *   `limitAngle` (擺動角度上限，預設 `45.0` 度)：擺錘從中線垂直位置到最左（右）側的最大偏離夾角。
    *   `speed` (晃動速度，預設 `2.0`)：擺動的頻率。數值越高擺得越快。
    *   `timeOffset` (時間相位偏移，預設 `0.0` 秒)：可用於設定一排擺錘時，修改每個擺錘的 offset 值，即可排布出波浪式交錯擺動效果。
    *   `swingAxis` (擺動旋轉軸，預設 `Z`)：繞行旋轉之軸向。可選擇 X、Y 或 Z（視乎模型在場景中的方位而定，預設為 Z 軸）。

---

### 14. `Checkpoint.cs` (存檔記錄點)
*   **用途**：掛載在發光台座或區域上。當球體經過，會更新 `Player` 最新的傳送復活位置，同時將自身變為綠色，並重置場景中其他舊記錄點。
*   **前置作業**：
    *   Collider 勾選或不勾選 `Is Trigger` 皆可。
*   **Inspector 屬性詳細說明**：
    *   `changeColor` (是否改變材質顏色，預設 `true`)：被玩家啟用時，本體是否自動改變顏色提供反饋。
    *   `activeColor` (啟用顏色，預設 `Green 綠色`)：啟用時開關本體 MeshRenderer 變更的新顏色。
    *   `playerTag` (玩家的標籤，預設 `Player`)。

---

### 15. `MoveForceModifier.cs` (移動力道調整/狀態道具)
*   **用途**：狀態藥水。吃到後，以乘數方式放大或縮小玩家的移動施力大小，時效結束後還原。
*   **前置作業**：
    *   物件的 Collider **必須勾選 `Is Trigger`**。
*   **Inspector 屬性詳細說明**：
    *   `forceMultiplier` (移動力道倍率，預設 `2.0`)：
        *   設為 `2`：移動推力變為兩倍（加速鞋）。
        *   設為 `0.5`：移動推力減半（黏滯泥沼）。
    *   `duration` (效果持續時間，預設 `5.0` 秒)：改變玩家力道的秒數。若設為 `0` 或負數，則永久改變直到陣亡重啟。
    *   `respawnDelay` (道具重生冷卻，預設 `3.0` 秒)：被玩家吃掉後到道具原處浮現的秒數（若勾選了 `destroyAfterUse`，此欄無效）。
    *   `destroyAfterUse` (一次性道具，預設 `false`)：
        *   若為 `true`：玩家吃掉道具後，道具直接自我銷毀（Destroy），不會重生（如果是暫時性道具，會等持續時間結束、將玩家力道還原後才銷毀本體）。
        *   若為 `false`：吃掉後會依照 `respawnDelay` 秒數原地重生，可重複拾取。
    *   `playerTag` (玩家的標籤，預設 `Player`)。

---

## 👩‍🏫 課堂教學常見除錯（Q&A）

*   **Q：為什麼我的球碰到任何機關（加速、跳板、傳送門、終點）都完全沒有反應？**
    *   **A**：請檢查主角球體 (Player) 的 **Tag 標籤** 是否已設為 **`Player`**。Unity 預設新建的物體 Tag 為 `Untagged`，必須手動改寫。
*   **Q：為什麼我的球滾不上去移動平台或塌陷地板，而是直接穿透掉了下去？**
    *   **A**：請確保平台或地板物件的 Collider 組件中，**`Is Trigger` 沒有被勾選**。只有作為「穿越型」觸發機關（如加速、傳送門、終點）時，才需要勾選 `Is Trigger`。
*   **Q：為什麼我設定了記錄點（Checkpoint），但是球掉下去後還是重新載入整關，沒有在記錄點復活？**
    *   **A**：請檢查您球上的 `FallDetector` 或尖刺上的 `Hazard`，必須**將 `Reload Scene` 取消勾選**，傳送復活機制才會啟動。

---

## 🛠️ 進階手感調校與專案設定

為了讓課堂教學效果更上一層樓，以下提供兩個能大幅提升遊戲精緻度的進階調校技巧：

### 1. 🟢 物理材質 (Physics Material) 的妙用（控制打滑與反彈）
滾球遊戲的「手感」完全取決於物理材質的設定。您可引導學生建立不同的物理材質以模擬特殊路面：
*   **建立方式**：在 Project 視窗點擊右鍵 -> **Create -> 3D -> Physics Material**。
*   **參數調校**：
    *   **冰面 (Ice)**：將 `Dynamic Friction` (動態摩擦力) 與 `Static Friction` (靜態摩擦力) 設為 `0.01` ~ `0.05`。球滾上去會產生極佳的打滑效果。
    *   **彈力蹦蹦面 (Bouncy Surface)**：將 `Bounciness` (彈性) 設為 `0.8` ~ `1.0`，並將 `Bounciness Combine` 設為 `Maximum`。球體撞擊此牆面或地面時會產生強力反彈。
    *   **一般路面**：摩擦力建議設為 `0.4` ~ `0.6`，提供穩定的控球感。
*   **套用方法**：將建好的物理材質檔案，直接拖入玩家球體或地面 Cube 身上 **Collider 組件中的 `Material` 欄位** 即可。

### 2. 🎮 新版 Input System 的相容設定
本工具箱的 `PlayerController.cs` 預設採用 Unity 的新版 **Input System**：
*   若專案編譯時出現 `UnityEngine.InputSystem` 相關的紅字報錯，代表專案尚未啟用該套件。
*   **解決方式**：
    1. 至 **Window -> Package Manager** 搜尋並安裝 **Input System**。
    2. 安裝後重啟 Unity，或至 **Edit -> Project Settings -> Player -> Other Settings**。
    3. 尋找 **Active Input Handling**，將其切換為 **`Both`** 或 **`Input System Package (New)`**，即可正常編譯運作。

### 3. 🏁 多關卡 Build Settings 專案設定指引
當您在關卡終點 (`GoalTrigger`) 啟用自動轉場載入下一關時，必須先在 Unity 的建置設定中註冊這些場景，否則會出現 `Scene cannot be loaded` 的運行錯誤。
*   **設定步驟**：
    1. 點選 Unity 頂部選單中的 **File -> Build Settings**。
    2. 在彈出的視窗上方，會看到一個 **`Scenes In Build`** (打包場景清單) 的空白區域。
    3. **直接將 Project 視窗中的關卡場景檔案**（例如：`Level1.unity`、`Level2.unity`、`Level3.unity`）**拖曳**到該區域中。
    4. **排列順序（重要）**：清單右側會顯示索引值 (Index，如 `0`, `1`, `2`...)。請將第一關拖曳排在最上方（Index 0），第二關排在其下方（Index 1），以此類推。
    *   *註：當 `GoalTrigger` 的 `nextSceneName` 保持留空時，腳本會自動抓取「當前關卡的 Index + 1」來作為載入目標，因此在 Build Settings 中的排列順序即代表遊戲的闖關順序。*

### 4. 🎵 遊戲背景音樂 (BGM) 的極簡設定教學
合適的背景音樂能讓滾球遊戲的體驗生色不少。在 Unity 中建立循環背景音樂非常簡單：
*   **設定步驟**：
    1. 在 Hierarchy 視窗空白處點擊滑鼠右鍵，選擇 **`Create Empty`**（建立空物件），並將其重新命名為 **`BGM_Player`**。
    2. 點選 `BGM_Player`，在右側 Inspector 面板點擊 **`Add Component`**，搜尋並掛載 **`Audio Source`** (音訊源組件)。
    3. 將您匯入專案的音樂檔案（支援 `.mp3`, `.wav`, `.ogg` 等格式）拖入 Audio Source 組件最上方的 **`AudioClip`** 欄位中。
    4. **屬性調整（關鍵）**：
       *   **`Play On Awake` (啟動時播放)**：**必須勾選**（確保關卡一載入音樂就會自動響起）。
       *   **`Loop` (循環播放)**：**必須勾選**（確保音樂播完後會自動重播，不會中斷）。
       *   **`Volume` (音量)**：建議調整至 **`0.2` ~ `0.4`** 左右（背景音樂音量適中即可，過大會干擾遊戲體驗）。
       *   **`Spatial Blend` (空間混合)**：保持在最左側的 **`0` (完全 2D)**。這樣背景音樂就會以穩定的立體聲播放，不會因為相機移動而產生忽大忽小或左右偏音的問題。

---

## 📂 專案目錄結構 (Project Directory Structure)

當您將這些檔案放到 GitHub 時，建議保持以下目錄結構，以確保自訂編輯器面板 (Custom Editor) 能夠正確運行：

```text
Assets/
└── Scripts/
    ├── Editor/                    # 📌 存放自訂 Inspector 編輯器擴充腳本 (必備)
    │   ├── GoalTriggerEditor.cs
    │   ├── PathMovementEditor.cs
    │   └── TriggerSwitchEditor.cs
    │
    ├── CameraFollow.cs            # 🕹️ 核心跟隨與控制腳本
    ├── Checkpoint.cs
    ├── CrumblingPlatform.cs
    ├── FallDetector.cs
    ├── GoalTrigger.cs
    ├── Hazard.cs
    ├── JumpPad.cs
    ├── MoveForceModifier.cs
    ├── PathMovement.cs
    ├── Pendulum.cs
    ├── PlayerController.cs
    ├── RotatingObstacle.cs
    ├── SpeedBoostItem.cs
    ├── Teleporter.cs
    └── TriggerSwitch.cs
```

---

## 📄 授權與開放使用聲明 (License)

本專案採用 **MIT 授權條款** 開放授權。
歡迎自由下載、修改、重製、或將本工具箱整合至您個人的教學課程、教材、工作坊或遊戲開發專案中。
若本專案對您的學習或教學有所幫助，也歡迎在您的 Repository 中附上本專案的 GitHub 網址。祝您學習愉快！
