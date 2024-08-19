using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructor : MonoBehaviour
{
    public GameObject instructionA;
    public GameObject instructionB;

    public void ShowInstructions()
    {
        // Time.timeScale = 0;
        // StartCoroutine(ShowInstructionsCoroutine());
    }

    public IEnumerator ShowInstructionsCoroutine()
    {
        float elapsedTime = 0f;
        bool showA = true;

        while (elapsedTime < 3f)
        {
            // 交替显示指示物
            instructionA.SetActive(showA);
            instructionB.SetActive(!showA);

            // 等待0.3秒
            yield return new WaitForSecondsRealtime(0.3f);

            // 切换显示的指示物
            showA = !showA;
            elapsedTime += 0.3f;
        }

        // 3秒后隐藏指示物
        instructionA.SetActive(false);
        instructionB.SetActive(false);

        // 恢复游戏
        Time.timeScale = 1;
    }
}