using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class SectorCollider2D : MonoBehaviour
{
    [SerializeField] public float radius; // 扇形半径
    [SerializeField] public float angle; // 扇形角度
    [SerializeField] public int segmentCount; // 分段数，影响顶点数量
    [SerializeField] public Vector2 direction; // 扇形的朝向

    private PolygonCollider2D polygonCollider;
    private LampController lampController;
    private List<GameObject> bugsInCollider;

    void Start()
    {
        bugsInCollider = new List<GameObject>();
        segmentCount = 10;
        angle = 60f;
        direction = Vector2.up;
        lampController = GetComponentInParent<LampController>();
        if (lampController == null)
        {
            Debug.LogError("LampController not found on parent object.");
        }
        else
        {
            radius = 0.8f * Mathf.Max(lampController.CurrentRange - 1f, 0);
            polygonCollider = GetComponent<PolygonCollider2D>();
            UpdateCollider();
        }
    }

    
    // 当有GameObject进入触发器范围时调用
    void OnTriggerEnter2D(Collider2D other)
    {
        // 将进入的GameObject添加到列表中
        if (!bugsInCollider.Contains(other.gameObject) && other.name.Contains("Phantom"))
        {
            bugsInCollider.Add(other.gameObject);
            Debug.Log("Object entered: " + other.gameObject.name);
        }
    }

    // 当有GameObject离开触发器范围时调用
    void OnTriggerExit2D(Collider2D other)
    {
        // 将离开的GameObject从列表中移除
        if (bugsInCollider.Contains(other.gameObject))
        {
            bugsInCollider.Remove(other.gameObject);
            Debug.Log("Object exited: " + other.gameObject.name);
        }
    }

    // 可以在其他地方访问当前在触发器内的所有GameObject
    public GameObject[] GetObjectsInTrigger()
    {
        return bugsInCollider.ToArray();
    }
    
    void Update()
    {
        if (lampController != null)
        {
            radius = 0.8f * Mathf.Max(lampController.CurrentRange - 1f, 0);
            angle = 0.8f * Mathf.Max(lampController.lampLight.pointLightInnerAngle, 0);
            UpdateCollider();
        }
    }

    void UpdateCollider()
    {
        if (lampController == null) return;

        // 扇形顶点数量 = 分段数 + 2（扇形圆心和末端点）
        Vector2[] points = new Vector2[segmentCount + 2];
        points[0] = Vector2.zero; // 圆心

        float halfAngle = angle / 2f;
        float angleStep = angle / segmentCount;
        float rotationAngle = Mathf.Atan2(direction.y, direction.x);

        // 计算每个顶点的位置
        for (int i = 0; i <= segmentCount; i++)
        {
            float currentAngle = -halfAngle + angleStep * i;
            float rad = Mathf.Deg2Rad * currentAngle + rotationAngle;
            Vector2 point = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;
            points[i + 1] = point;
        }

        // 设置碰撞器的顶点
        polygonCollider.points = points;
    }

    void OnValidate()
    {
        if (polygonCollider == null)
            polygonCollider = GetComponent<PolygonCollider2D>();
        UpdateCollider();
    }
}
