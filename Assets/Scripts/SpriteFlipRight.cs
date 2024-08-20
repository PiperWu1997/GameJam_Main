using UnityEngine;

public class SpriteFlipRight : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        FlipSpriteOnScreenEdge();
    }

    void FlipSpriteOnScreenEdge()
    {
        // 获取屏幕的中心位置
        float screenCenterX = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, 0, 0)).x;

        // 获取当前物体的位置
        float spritePositionX = transform.position.x;

        // 如果 sprite 位于屏幕的左侧
        if (spritePositionX < screenCenterX && transform.localScale.x > 0)
        {
            // 反转 sprite 的 x 轴 scale
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        // 如果 sprite 位于屏幕的右侧
        else if (spritePositionX >= screenCenterX && transform.localScale.x < 0)
        {
            // 将 sprite 的 x 轴 scale 改回正值
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
}
