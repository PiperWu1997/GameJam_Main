using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DissolveExample
{
    public class DissolveChilds : MonoBehaviour
    {
        List<Material> materials = new List<Material>();
        bool dissolveFinished = false; // 标志溶解是否已完成

        void Start()
        {
            var renders = GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renders.Length; i++)
            {
                materials.AddRange(renders[i].materials);
            }

            SetInitialDissolveValue(0); // 设置初始溶解值为0
        }

        private void Reset()
        {
            Start();
        }

        public void SetInitialDissolveValue(float value)
        {
            SetValue(value); // 设置初始溶解值
        }

        void Update()
        {
            if (!dissolveFinished)
            {
                var value = Mathf.PingPong(Time.time * 0.1f, 1f);
                SetValue(value);

                // 当溶解值达到1时，销毁物体
                if (value >= 1f)
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
}
