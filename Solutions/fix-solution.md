这个 demo 本质上就是一个**刚体 + 重力 + 倾斜控制**的小物理系统，Unity 自带的物理引擎已经帮你完成了 90% 的工作，你只需要把“倾斜”控制好就行。

我给你一步一步搭一个**最干净、不会踩坑的实现方案（基于新 Input System 也适用）**。

---

# 🧱 一、场景搭建

### 1. 创建木板（Plane / Cube 都行）

* GameObject → 3D Object → Cube
* 命名：`Board`
* Scale：比如 `(5, 0.2, 5)`（做成一个薄板）

👉 添加组件：

* ✅ `Box Collider`（默认有）
* ❌ 不要加 Rigidbody（否则它会掉下去）

---

### 2. 创建小球

* GameObject → 3D Object → Sphere
* 命名：`Ball`
* 放在木板上方一点点（例如 y = 1）

👉 添加组件：

* ✅ `Rigidbody`
* ✅ `Sphere Collider`（默认有）

👉 Rigidbody 参数建议：

* Mass: 1
* Drag: 0.2（轻微阻尼，防止无限滚）
* Angular Drag: 0.05
* Use Gravity: ✅
* Interpolate: Interpolate（更顺滑）

---

# 🎮 二、核心思路（非常重要）

👉 不要控制球
👉 **只控制木板旋转**

物理系统会自动帮你实现：

* 倾斜 → 重力分解 → 球滚动
* 倾斜越大 → 加速度越大 → 速度越快

---

# 🧠 三、控制脚本（BoardController）

挂在 `Board` 上：

```csharp
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public float tiltSpeed = 50f;   // 倾斜速度
    public float maxTilt = 15f;     // 最大倾斜角度

    private float tiltX; // 前后
    private float tiltZ; // 左右

    void Update()
    {
        // 获取输入（新旧 Input System 都兼容的写法）
        float inputX = Input.GetAxis("Horizontal"); // A/D 或 左右
        float inputZ = Input.GetAxis("Vertical");   // W/S 或 上下

        // 累积角度
        tiltX += inputZ * tiltSpeed * Time.deltaTime;
        tiltZ -= inputX * tiltSpeed * Time.deltaTime;

        // 限制最大角度
        tiltX = Mathf.Clamp(tiltX, -maxTilt, maxTilt);
        tiltZ = Mathf.Clamp(tiltZ, -maxTilt, maxTilt);

        // 应用旋转
        transform.rotation = Quaternion.Euler(tiltX, 0, tiltZ);
    }
}
```

---

# ⚙️ 四、如果你用的是“新 Input System”（你之前遇到的报错）

你现在很可能已经启用了：

> Input System Package（新系统）

那 `Input.GetAxis()` 会报错。

👉 解决方案有两个：

---

## ✅ 方案 A（最简单）

去：

```
Edit → Project Settings → Player → Active Input Handling
```

改成：

```
Both
```

✔ 这样旧代码直接可用（推荐你先用这个跑通 demo）

---

## ✅ 方案 B（纯新 Input System 写法）

用 `InputAction`：

```csharp
using UnityEngine;
using UnityEngine.InputSystem;

public class BoardController : MonoBehaviour
{
    public float tiltSpeed = 50f;
    public float maxTilt = 15f;

    private float tiltX;
    private float tiltZ;

    private Vector2 input;

    public void OnMove(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    void Update()
    {
        tiltX += input.y * tiltSpeed * Time.deltaTime;
        tiltZ -= input.x * tiltSpeed * Time.deltaTime;

        tiltX = Mathf.Clamp(tiltX, -maxTilt, maxTilt);
        tiltZ = Mathf.Clamp(tiltZ, -maxTilt, maxTilt);

        transform.rotation = Quaternion.Euler(tiltX, 0, tiltZ);
    }
}
```

然后在 Input Actions 里绑定 WASD / 手柄。

---

# 🧪 五、让效果更“像游戏”的优化

### 1. 增加摩擦（防止球太滑）

给木板和球加 Physics Material：

* Friction: 0.6 ~ 1
* Bounciness: 0

---

### 2. 平滑倾斜（更高级）

把 `Update()` 改成：

```csharp
Quaternion targetRotation = Quaternion.Euler(tiltX, 0, tiltZ);
transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
```

---

### 3. 限制球速度（防飞出去）

```csharp
Rigidbody rb = ball.GetComponent<Rigidbody>();

if (rb.velocity.magnitude > 10f)
{
    rb.velocity = rb.velocity.normalized * 10f;
}
```

---

# 🎯 六、核心物理原理（你可以理解一下）

当板子倾斜角度为 θ 时：

👉 重力分解为沿斜面的分量：

```
a = g * sin(θ)
```

👉 所以：

* 倾斜越大 → sin(θ) 越大 → 加速度越大
* 球自然滚得更快

这就是你想要的“**倾斜越大 → 速度越快**”的本质原因。

---

# ✅ 最终效果

你会得到一个类似：

* 《超级猴子球》
* 《平衡球》

的基础玩法 demo 🎮

---

如果你下一步想升级这个 demo，我可以帮你继续扩展：

👉 加：

* 🎯 终点 + 计时系统
* 🕳 掉落判定
* 📱 手机陀螺仪控制（超酷）
* 🧠 AI 自动走迷宫

直接说你想做到哪一步，我可以帮你做成一个完整小项目 👍
