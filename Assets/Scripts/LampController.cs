using UnityEngine;

public class LampController : MonoBehaviour
{
    public Light2D lampLight;  // 如果使用2D光照系统，可以添加Light2D组件
    public float maxIntensity = 5f;  // 灯的最大亮度
    public float minIntensity = 0.5f;  // 灯的最小亮度
    public float maxRange = 5f;  // 灯光的最大范围
    public float minRange = 1f;  // 灯光的最小范围

    void Update()
    {
        // 控制灯光的亮度和范围
        if (Input.GetMouseButton(0))  // 持续按住鼠标左键
        {
            lampLight.intensity = maxIntensity;
            lampLight.pointLightOuterRadius = maxRange;
        }
        else
        {
            lampLight.intensity = minIntensity;
            lampLight.pointLightOuterRadius = minRange;
        }

        // 控制灯光方向
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);  // -90调整角度
    }
}
