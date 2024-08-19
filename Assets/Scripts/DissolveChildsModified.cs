using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DissolveChildsModified : MonoBehaviour
{
    List<Material> materials;
    bool dissolveFinished; // 标志溶解是否已完成
    private float dissolveValue;
    public float dissolveSpeed = 0.2f;

    void Start()
    {
        dissolveValue = 0;
        materials = new List<Material>();
        dissolveFinished = false;
        var renders = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renders.Length; i++)
        {
            materials.AddRange(renders[i].materials);
        }

        SetInitialDissolveValue(0); // 设置初始溶解值为0
    }

    public void SetInitialDissolveValue(float value)
    {
        SetValue(value); // 设置初始溶解值
    }

    void Update()
    {
        if (!dissolveFinished)
        {
            dissolveValue += Time.deltaTime * dissolveSpeed;
            SetValue(dissolveValue);

            // 当溶解值达到1时，销毁物体
            if (dissolveValue >= 1f)
            {
                dissolveFinished = true;
                Destroy(gameObject); // 销毁脚本所附加的物体
            }
        }
    }

    public void SetValue(float value)
    {
        for (int i = 0; i < materials.Count; i++)
        {
            materials[i].SetFloat("_Dissolve", value);
        }
    }
}