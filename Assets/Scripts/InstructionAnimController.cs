using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InstructionAnimController : MonoBehaviour
{
    public Animator firstAnimator; // 第一个 Animator 组件
    public Animator secondAnimator; // 第二个 Animator 组件
    public Button nextButton; // UI Button

    private bool firstAnimationPlayed = false;
    private int count;
    void Start()
    {
        count = 0;
        // 禁用 Animator，确保动画不会自动播放
        firstAnimator.enabled = true;
        // firstAnimator.Play("Insturction_1");
        secondAnimator.enabled = false; // 订阅按钮点击事件
        nextButton.onClick.AddListener(OnNextButtonClicked);
    }

    private void OnNextButtonClicked()
    {
        if (count == 0)
        {
            firstAnimator.enabled = false; // 禁用第一个 Animator
            // 启用第二个 Animator 并播放第二个动画
            secondAnimator.enabled = true;
            // secondAnimator.Play("Instruction_2");
            count += 1;
        }
        else
        {
            SceneManager.LoadScene("MainScene");
        }
       
    }

    private IEnumerator WaitForFirstAnimationToEnd()
    {
        // 等待第一个动画播放完毕
        AnimatorStateInfo stateInfo = firstAnimator.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;
        yield return new WaitForSeconds(animationLength);

        // 标记第一个动画已播放完毕
        firstAnimationPlayed = true;
    }
}