using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Test_LampController1 : MonoBehaviour
{
    public UnityEngine.Rendering.Universal.Light2D lampLight;  // Light2D 组件的引用
    public float maxIntensity = 5f;  // 灯光的最大强度
    public float minIntensity = 0f;  // 灯光的最小强度
    public float maxRange = 5f;  // 灯光的最大范围
    public float minRange = 0f;  // 灯光的最小范围
    public float intensityIncreaseRate = 1f;  // 强度增加的速率
    public float rangeIncreaseRate = 1f;  // 范围增加的速率
    public float smoothTime = 0.5f;  // 用于平滑减少强度和范围的时间
    public float rotationSmoothTime = 0.1f;  // 灯光旋转的平滑时间
    public float batteryConsumptionRate = 10f;  // 电池消耗速率
    public float maxBattery = 100f;  // 最大电池电量
    public Slider batterySlider;  // 电池滑动条的引用
    public float batteryIncreaseAmount = 20f;  // 电池增加的量

    private float currentIntensity;  // 当前强度
    public float currentRange;  // 当前范围
    private float currentBattery;  // 当前电池电量
    private float intensityVelocity = 0f;  // 强度平滑的速度
    private float rangeVelocity = 0f;  // 范围平滑的速度
    private Vector3 currentVelocity = Vector3.zero;  // 旋转平滑的速度

    public float laserInnerAngle = 0f; // 激光模式内角
    public float laserOuterAngle = 8f; // 激光模式外角
    private float defaultInnerAngle; // 默认内角
    private float defaultOuterAngle; // 默认外角

    private bool isLaserMode = false; // 当前是否为激光模式

    void Start()
    {
        currentBattery = maxBattery;  // 将电池电量初始化为最大值
        batterySlider.maxValue = maxBattery;  // 设置滑动条的最大值
        batterySlider.value = currentBattery;  // 初始化滑动条的值

        // 保存默认的内角和外角
        defaultInnerAngle = lampLight.pointLightInnerAngle;
        defaultOuterAngle = lampLight.pointLightOuterAngle;
    }

    void Update()
    {
        // 切换到激光模式
        if (Input.GetMouseButton(0)) // 按住左键时
        {
            // 增加灯光的强度和范围
            currentIntensity = Mathf.Clamp(currentIntensity + intensityIncreaseRate * Time.deltaTime, minIntensity, maxIntensity);
            currentRange = Mathf.Clamp(currentRange + rangeIncreaseRate * Time.deltaTime, minRange, maxRange);

            // 根据强度计算电池消耗
            float batteryConsumption = batteryConsumptionRate * currentIntensity * Time.deltaTime;
            currentBattery = Mathf.Clamp(currentBattery - batteryConsumption, 0, maxBattery);

            // 更新电池滑动条
            batterySlider.value = currentBattery;

            // 切换到激光模式
            if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.Space))
            {
                // 设置灯光为激光模式
                lampLight.pointLightInnerAngle = Mathf.Lerp(lampLight.pointLightInnerAngle, laserInnerAngle, Time.deltaTime * 10f);
                lampLight.pointLightOuterRadius = Mathf.Lerp(lampLight.pointLightOuterRadius, laserOuterAngle, Time.deltaTime * 10f);
                isLaserMode = true;
            }
            else
            {
                // 恢复到正常模式
                lampLight.pointLightInnerAngle = Mathf.Lerp(lampLight.pointLightInnerAngle, defaultInnerAngle, Time.deltaTime * 10f);
                lampLight.pointLightOuterRadius = Mathf.Lerp(lampLight.pointLightOuterRadius, defaultOuterAngle, Time.deltaTime * 10f);
                isLaserMode = false;
            }
        }
        else
        {
            // 当鼠标没有按下时，平滑地减少灯光强度和范围
            currentIntensity = Mathf.SmoothDamp(currentIntensity, minIntensity, ref intensityVelocity, smoothTime);
            currentRange = Mathf.SmoothDamp(currentRange, minRange, ref rangeVelocity, smoothTime);
        }

        // 应用灯光的强度和范围
        lampLight.intensity = currentIntensity;
        lampLight.pointLightOuterRadius = currentRange;

        // 控制灯光方向
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 平滑旋转灯光
        float smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.z, targetAngle - 90, ref currentVelocity.z, rotationSmoothTime);
        transform.rotation = Quaternion.Euler(0, 0, smoothedAngle);

        // 检测电池电量是否为 0
        if (batterySlider.value <= 0)
        {
            LoadNextLevel();
        }
    }

    public void DecreaseBattery(float amount)
    {
        currentBattery = Mathf.Clamp(currentBattery - amount, 0, maxBattery);
        batterySlider.value = currentBattery;

        // 处理电池耗尽逻辑
        if (currentBattery <= 0)
        {
            // 例如，关闭灯光
            lampLight.enabled = false;
            LoadNextLevel();  // 电池耗尽时也可以调用加载下一关
        }
    }

    public void IncreaseBattery(float amount)
    {
        currentBattery = Mathf.Clamp(currentBattery + amount, 0, maxBattery);
        batterySlider.value = currentBattery;
    }

    private void LoadNextLevel()
    {
        // 加载下一关卡，这里可以通过名字或索引加载
        UnityEngine.SceneManagement.SceneManager.LoadScene("EndScene");  // 将 "EndScene" 替换为您实际的下一关的场景名
    }

    public bool IsLaserMode()
    {
        return isLaserMode;
    }
}
