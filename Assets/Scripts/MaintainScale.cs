using UnityEngine;

public class MaintainScale : MonoBehaviour
{
    private Vector3 originalScale;

    void Start()
    {
        // 存储子物体的初始缩放
        originalScale = transform.localScale;
    }

    void Update()
    {
        // 通过反向调整缩放，使子物体的缩放保持恒定
        Vector3 parentScale = transform.parent != null ? transform.parent.localScale : Vector3.one;
        transform.localScale = new Vector3(
            originalScale.x / parentScale.x,
            originalScale.y / parentScale.y,
            originalScale.z / parentScale.z
        );
    }
}