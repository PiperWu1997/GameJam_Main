using UnityEngine;
using UnityEngine.Rendering.Universal;


public class LampController : MonoBehaviour
{
    public UnityEngine.Rendering.Universal.Light2D lampLight;  // 引用Light2D组件
    public float maxIntensity = 5f;  // 灯的最大亮度
    public float minIntensity = 0f;  // 灯的最小亮度
    public float maxRange = 5f;  // 灯光的最大范围
    public float minRange = 0f;  // 灯光的最小范围
    public float intensityIncreaseRate = 1f;  // 亮度增加的速度
    public float rangeIncreaseRate = 1f;  // 范围增加的速度
    public float smoothTime = 0.5f;  // 亮度和范围减少的平滑时间
    public float rotationSmoothTime = 0.1f;  // 灯光旋转的平滑时间

    private float currentIntensity;  // 当前亮度
    private float currentRange;  // 当前范围
    private float intensityVelocity = 0f;  // 亮度平滑的速度
    private float rangeVelocity = 0f;  // 范围平滑的速度
    private Vector3 currentVelocity = Vector3.zero;  // 旋转平滑的速度

    void Update()
    {
        if (Input.GetMouseButton(0))  // 持续按住鼠标左键
        {
            // 增加灯光的亮度和范围
            currentIntensity = Mathf.Clamp(currentIntensity + intensityIncreaseRate * Time.deltaTime, minIntensity, maxIntensity);
            currentRange = Mathf.Clamp(currentRange + rangeIncreaseRate * Time.deltaTime, minRange, maxRange);
        }
        else
        {
            // 松开鼠标时缓慢减少灯光的亮度和范围
            currentIntensity = Mathf.SmoothDamp(currentIntensity, minIntensity, ref intensityVelocity, smoothTime);
            currentRange = Mathf.SmoothDamp(currentRange, minRange, ref rangeVelocity, smoothTime);
        }

        // 应用灯光的亮度和范围
        lampLight.intensity = currentIntensity;
        lampLight.pointLightOuterRadius = currentRange;

        // 控制灯光方向
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 平滑旋转灯光
        float smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.z, targetAngle - 90, ref currentVelocity.z, rotationSmoothTime);
        transform.rotation = Quaternion.Euler(0, 0, smoothedAngle);
    }
}
