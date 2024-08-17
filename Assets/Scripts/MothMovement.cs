using UnityEngine;

public class MothMovement : FlyMovement
{
    public float damageToLight = 10f; // 飞蛾对灯造成的伤害
    private LampController lampController; // 引用 LampController

    void Start()
    {
        // 查找 LampController 组件
        lampController = FindObjectOfType<LampController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("LampLight"))
        {
            if (lampController != null)
            {
                lampController.DecreaseBattery(damageToLight); // 调用减少电量的方法
            }

            Destroy(gameObject); // 销毁飞蛾对象
        }
    }
}
