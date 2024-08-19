using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LampController : MonoBehaviour
{
    public UnityEngine.Rendering.Universal.Light2D lampLight;
    public float maxIntensity = 5f;
    public float minIntensity = 0f;
    public float maxRange = 5f;
    public float minRange = 0f;
    public float intensityIncreaseRate = 1f;
    public float rangeIncreaseRate = 1f;
    public float smoothTime = 0.5f;
    public float rotationSmoothTime = 0.1f;
    public float batteryConsumptionRate = 10f;
    public float maxBattery = 100f;
    public Slider batterySlider;

    public float batteryIncreaseAmount = 20f;
    public float laserInnerAngle = 0f;
    public float laserOuterAngle = 8f;
    public float angleSmoothTime = 0.2f;

    public float CurrentRange { get; private set; }
    public float CurrentBattery { get; private set; }

    private float currentIntensity;
    [SerializeField] private float currentBattery;
    private float intensityVelocity = 0f;
    private float rangeVelocity = 0f;
    private float innerAngleVelocity = 0f;
    private float outerAngleVelocity = 0f;
    private Vector3 currentVelocity = Vector3.zero;

    private float initialInnerAngle;
    private float initialOuterAngle;

    private bool isWaitingForFlashLightSecondClick;
    private float flashLightTimer;
    private float flashLightTimeWindow; // 0.5秒的时间窗口
    private bool flashLightSkillReleased;
    private PolygonCollider2D detectorCollider2D;
    private SectorCollider2D sectorCollider2D;

    public int flashCount ; // 闪烁次数
    public float flashDuration; // 每次闪烁的持续时间

    void Start()
    {
        flashCount = 4;
        flashDuration = 0.1f;
        flashLightTimer = 0f;
        flashLightTimeWindow = 0.5f;
        isWaitingForFlashLightSecondClick = false;
        currentBattery = maxBattery;
        batterySlider.maxValue = maxBattery;
        batterySlider.value = currentBattery;
        flashLightSkillReleased = false;
        initialInnerAngle = lampLight.pointLightInnerAngle;
        initialOuterAngle = lampLight.pointLightOuterAngle;
        detectorCollider2D = transform.Find("Detector").GetComponent<PolygonCollider2D>();
        sectorCollider2D = transform.Find("Detector").GetComponent<SectorCollider2D>();
    }

    void Update()
    {
        // 首先更新灯光方向，以确保灯光指向鼠标
        UpdateLightDirection();

        // 然后处理灯光的控制逻辑，包括强度、范围和电池消耗
        HandleLightControl();
        
        // 处理闪光灯技能
        HandleFlashLightSkill();
        
        // 最后处理激光模式和射线检测
        if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.Space))
        {
            EnterLaserMode();
            HandleLaserRaycast();
        }
        else
        {
            ExitLaserMode();
        }

        // 检查电池状态
        if (batterySlider.value <= 0)
        {
            LoadNextLevel();
        }
    }

    private void HandleFlashLightSkill()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (!isWaitingForFlashLightSecondClick)
            {
                // 第一次松开左键，开始计时
                isWaitingForFlashLightSecondClick = true;
                flashLightSkillReleased = false;
                flashLightTimer = 0.0f;
            }
            else if (!flashLightSkillReleased && flashLightTimer <= flashLightTimeWindow)
            {
                // 在0.5秒内再次点击左键，则释放技能
                StartCoroutine(ReleaseFlashLightSkill());
                flashLightSkillReleased = true;
            }
        }

        // 计时器运行
        if (isWaitingForFlashLightSecondClick)
        {
            flashLightTimer += Time.deltaTime;

            if (flashLightTimer > flashLightTimeWindow)
            {
                // 超过0.5秒后重置状态
                isWaitingForFlashLightSecondClick = false;
                flashLightSkillReleased = false;
            }
        }
    }

    private IEnumerator ReleaseFlashLightSkill()
    {
        Debug.Log("Flashlight Skill Released!");

        for (int i = 0; i < flashCount; i++)
        {
            // 关闭灯光
            lampLight.enabled = false;
            detectorCollider2D.enabled = false;
            yield return new WaitForSeconds(flashDuration);
            
            // 开启灯光
            lampLight.enabled = true;
            detectorCollider2D.enabled = true;
            yield return new WaitForSeconds(flashDuration);
        }

        // 确保最后一次闪烁后灯光是开启的
        lampLight.enabled = true;
        detectorCollider2D.enabled = true;
        foreach (var bug in sectorCollider2D.GetObjectsInTrigger())
        {
            Destroy(bug);
            Debug.Log($"Bug {bug} Destroyed!");
        }
    }

    void UpdateLightDirection()
    {
        // 计算鼠标位置和方向
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;

        // 计算目标角度并进行平滑旋转
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.z, targetAngle - 90, ref currentVelocity.z, rotationSmoothTime);
        transform.rotation = Quaternion.Euler(0, 0, smoothedAngle);
    }

    void HandleLightControl()
    {
        // 控制灯光的强度和范围
        if (Input.GetMouseButton(0))
        {
            currentIntensity = Mathf.Clamp(currentIntensity + intensityIncreaseRate * Time.deltaTime, minIntensity, maxIntensity);
            CurrentRange = Mathf.Clamp(CurrentRange + rangeIncreaseRate * Time.deltaTime, minRange, maxRange);

            float batteryConsumption = batteryConsumptionRate * currentIntensity * Time.deltaTime;
            currentBattery = Mathf.Clamp(currentBattery - batteryConsumption, 0, maxBattery);

            batterySlider.value = currentBattery;
        }
        else
        {
            currentIntensity = Mathf.SmoothDamp(currentIntensity, minIntensity, ref intensityVelocity, smoothTime);
            CurrentRange = Mathf.SmoothDamp(CurrentRange, minRange, ref rangeVelocity, smoothTime);
        }

        lampLight.intensity = currentIntensity;
        lampLight.pointLightOuterRadius = CurrentRange;
    }

    void EnterLaserMode()
    {
        // 进入激光模式，调整灯光角度
        lampLight.pointLightInnerAngle =
            Mathf.SmoothDamp(lampLight.pointLightInnerAngle, laserInnerAngle, ref innerAngleVelocity, angleSmoothTime);
        lampLight.pointLightOuterAngle =
            Mathf.SmoothDamp(lampLight.pointLightOuterAngle, laserOuterAngle, ref outerAngleVelocity, angleSmoothTime);
    }

    void ExitLaserMode()
    {
        // 退出激光模式，恢复灯光角度
        lampLight.pointLightInnerAngle =
            Mathf.SmoothDamp(lampLight.pointLightInnerAngle, initialInnerAngle, ref innerAngleVelocity, angleSmoothTime);
        lampLight.pointLightOuterAngle =
            Mathf.SmoothDamp(lampLight.pointLightOuterAngle, initialOuterAngle, ref outerAngleVelocity, angleSmoothTime);
    }

    void HandleLaserRaycast()
    {
        // 射线检测逻辑
        Vector2 raycastOrigin = lampLight.transform.position;
        Vector2 raycastDirection = lampLight.transform.up; // 使用灯光的up方向

        LayerMask beetleLayer = LayerMask.GetMask("BeetleLayer");

        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, raycastDirection, Mathf.Infinity, beetleLayer);

        if (hit.collider != null)
        {
            Beetle beetle = hit.collider.GetComponent<Beetle>();

            if (beetle != null)
            {
                beetle.IncreaseLaserExposure(Time.deltaTime);

                if (beetle.IsExposedToLaser(0.2f)) // 检查曝光时间是否超过阈值
                {
                    beetle.DestroyBeetle(true);
                    IncreaseBattery(batteryIncreaseAmount); // 增加电池
                }
            }
        }
    }

    public void DecreaseBattery(float amount)
    {
        currentBattery = Mathf.Clamp(currentBattery - amount, 0, maxBattery);
        batterySlider.value = currentBattery;

        if (currentBattery <= 0)
        {
            lampLight.enabled = false;
            LoadNextLevel();
        }
    }

    public void IncreaseBattery(float amount)
    {
        currentBattery = Mathf.Clamp(currentBattery + amount, 0, maxBattery);
        batterySlider.value = currentBattery;
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene("EndScene");
    }
}