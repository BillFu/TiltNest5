下面给你一套**从 0 到可运行 Demo 的 Unity 项目模板**（专为你的“重力小球”玩法设计）。你可以按步骤搭，或直接把代码粘进去就能跑。

---

# 🎮 0. 项目创建

在 Unity Hub 里新建：

* Template：**3D Core**
* 项目名：`GravityPockets`（或你选的名字）
* 路径：建议放外置 SSD（如 `/Volumes/DevSSD/UnityProjects/GravityPockets`）

---

# 🧱 1. 目录结构（先定好，后面不乱）

在 `Assets/` 下创建：

```text
Assets/
├── Scenes/
├── Scripts/
│   ├── Core/
│   ├── Gameplay/
│   └── Utils/
├── Prefabs/
├── Materials/
└── Physics/
```

---

# 🌄 2. 场景搭建（GameScene）

新建场景：`Assets/Scenes/GameScene.unity`

## 2.1 木板（Board）

* GameObject → 3D Object → **Cube**
* 命名：`Board`
* Scale：`(10, 0.5, 10)`
* 加组件：`BoxCollider`（默认即可）
* **不加 Rigidbody**

---

## 2.2 边界（Walls）

* 4 个薄 Cube 围住木板
* 高度略高于球（比如 y=1）
* 命名：`Wall_N/E/S/W`
* Collider 默认即可（不用 Rigidbody）

---

## 2.3 小球（Ball）

* 3D Object → **Sphere** × 5
* 命名：`Ball_1..5`
* Tag：新建 `Ball`
* 加组件：

  * `Rigidbody`
  * `SphereCollider`

👉 Rigidbody 参数建议：

```text
Mass: 1
Drag: 0.2
Angular Drag: 0.05
Collision Detection: Continuous
```

---

## 2.4 凹槽（Hole）

👉 用“视觉 + Trigger”模拟（不要真挖洞）

* 3D Object → **Sphere**（稍大一点作为视觉）
* 命名：`Hole_1..5`
* Scale：略大于球（如 1.2x）
* 再加一个子物体（或直接在本体上）：

  * `SphereCollider` → 勾选 **Is Trigger**

---

## 2.5 木柱（Obstacles）

* 3D Object → **Cylinder**
* 若干个，随意摆
* Collider 默认即可

---

## 2.6 相机（Camera）

* 位置：`(0, 15, -10)`
* 旋转：`(45, 0, 0)` 俯视
* 可微调视野（FOV）

---

# 📱 3. 倾斜控制（核心脚本）

创建：`Assets/Scripts/Core/BoardController.cs`

```csharp
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public float tiltSpeed = 15f;
    public float maxTilt = 15f;
    public float smooth = 5f;

    void Update()
    {
        Vector3 accel = Input.acceleration;

        float tiltX = Mathf.Clamp(accel.y * tiltSpeed, -maxTilt, maxTilt);
        float tiltZ = Mathf.Clamp(-accel.x * tiltSpeed, -maxTilt, maxTilt);

        Quaternion target = Quaternion.Euler(tiltX, 0, tiltZ);
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            target,
            Time.deltaTime * smooth
        );
    }
}
```

👉 挂到 `Board` 上

---

# 🧲 4. 凹槽吸附（核心玩法）

创建：`Assets/Scripts/Gameplay/Hole.cs`

```csharp
using UnityEngine;

public class Hole : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ball")) return;

        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb == null) return;

        // 停止运动
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // 固定
        rb.isKinematic = true;

        // 吸附到中心
        other.transform.position = transform.position;

        // 通知
        Ball ball = other.GetComponent<Ball>();
        if (ball != null) ball.OnCaptured();
    }
}
```

👉 挂到每个 `Hole`（带 Trigger 的那个 Collider）上

---

# ⚙️ 5. 小球逻辑（被吸附 / 掉落）

创建：`Assets/Scripts/Gameplay/Ball.cs`

```csharp
using UnityEngine;

public class Ball : MonoBehaviour
{
    private bool finished = false;

    void Update()
    {
        if (finished) return;

        // 掉出边界
        if (transform.position.y < -5f)
        {
            finished = true;
            GameManager.Instance.OnBallLost(this);
            Destroy(gameObject);
        }
    }

    public void OnCaptured()
    {
        if (finished) return;

        finished = true;
        GameManager.Instance.OnBallCaptured(this);
    }
}
```

👉 挂到每个 Ball 上

---

# 🧠 6. 游戏管理（胜负逻辑）

创建：`Assets/Scripts/Core/GameManager.cs`

```csharp
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int totalBalls = 5;
    private int finishedBalls = 0;

    void Awake()
    {
        Instance = this;
    }

    public void OnBallCaptured(Ball ball)
    {
        finishedBalls++;
        CheckEnd();
    }

    public void OnBallLost(Ball ball)
    {
        finishedBalls++;
        CheckEnd();
    }

    void CheckEnd()
    {
        if (finishedBalls >= totalBalls)
        {
            Debug.Log("Game Over");
            // TODO: UI / Restart
        }
    }
}
```

👉 新建空物体 `GameManager`，挂上脚本

---

# 🧪 7. 物理材质（手感优化）

创建：`Assets/Physics/BallMaterial`

参数建议：

```text
Dynamic Friction: 0.4
Static Friction: 0.4
Bounciness: 0.1
Friction Combine: Average
Bounce Combine: Minimum
```

👉 应用到：

* 球
* 木板

---

# ▶️ 8. 运行与测试

## 编辑器内测试（无手机）

👉 临时用键盘模拟（可选加一段）：

```csharp
#if UNITY_EDITOR
float tiltX = Input.GetAxis("Vertical") * tiltSpeed;
float tiltZ = -Input.GetAxis("Horizontal") * tiltSpeed;
#endif
```

---

## 真机测试（iPhone / Android）

* Build Settings → iOS / Android
* iOS 需要 Xcode

---

# 🚀 9. 下一步可以加的功能

* 🎯 分数系统（几颗球进洞）
* ⏱️ 计时挑战
* 🎚️ 多关卡（不同布局）
* 🎵 音效（进洞/掉落）
* 📳 震动反馈

---

# 🧩 一句话总结

> 这个模板的核心是：
> **“用手机倾斜 → 控制木板 → 驱动物理系统 → 判定结果”**

---

如果你想，我可以下一步帮你升级这个模板：

👉 🔥 加 **关卡生成系统（自动摆柱子和洞）**
👉 🔥 加 **UI（开始/重开/得分）**
👉 🔥 或直接做成 **可上架 App Store 的完整版本**
