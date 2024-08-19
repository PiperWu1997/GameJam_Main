using UnityEngine;

public class Beetle : MonoBehaviour
{
    public float laserExposureTime = 0f;  // 当前暴露在激光下的时间
    public float requiredExposureTime = 1f;  // 需要暴露在激光下的时间
    public GameObject destructionParticlePrefab;  // 粒子效果的 prefab

    private LampController lampController;  // 引用 LampController 脚本

    private bool isLaserMode = false;  // 是否处于激光模式

    void Start()
    {
        lampController = FindObjectOfType<LampController>();  // 获取 LampController 实例
    }

    void Update()
    {
        // 如果激光模式激活且暴露时间达到了要求，则销毁甲虫
        if (isLaserMode && laserExposureTime >= requiredExposureTime)
        {
            DestroyBeetle(true);
        }
    }

    public void IncreaseLaserExposure(float deltaTime)
    {
        laserExposureTime += deltaTime;
    }

    public bool IsExposedToLaser(float requiredTime)
    {
        return laserExposureTime >= requiredTime;
    }

    public void DestroyBeetle(bool destroyedByLaser)
    {
        if (destroyedByLaser)
        {
            // 激活粒子效果
            if (destructionParticlePrefab != null)
            {
                GameObject particleInstance = Instantiate(destructionParticlePrefab, transform.position, Quaternion.identity);
                ParticleSystem particleSystem = particleInstance.GetComponent<ParticleSystem>();
                if (particleSystem != null)
                {
                    particleSystem.Play();
                    Destroy(particleInstance, particleSystem.main.duration + particleSystem.main.startLifetime.constantMax);  // 根据粒子的生命周期和持续时间来销毁
                }
            }

            lampController.IncreaseBattery(5f);  // 增加 lamp 电池
        }
        else
        {
            lampController.DecreaseBattery(5f);  // 减少 lamp 电池
        }

        Destroy(gameObject);  // 销毁甲虫
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Lamp"))
        {
            Debug.Log("Beetle collided with Lamp");

            // 如果甲虫与 Lamp 碰撞并且没有被激光摧毁，减少电池
            if (laserExposureTime < requiredExposureTime)
            {
                lampController.DecreaseBattery(5f);
            }

            DestroyBeetle(false);  // 在碰撞时销毁甲虫，但不增加电池
        }
    }

    public void SetLaserMode(bool isActive)
    {
        isLaserMode = isActive;

        // 在进入激光模式时重置暴露时间
        if (isActive)
        {
            laserExposureTime = 0f;
        }
    }
}
