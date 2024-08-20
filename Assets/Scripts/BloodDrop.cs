using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodDrop : MonoBehaviour
{
    public AudioSource audioSource; // 用于播放音乐的 AudioSource
    public AudioClip musicClip; // 要播放的音频片段
    public float delay = 0.4f; // 延迟时间（秒）

    void Start()
    {
        // 检查是否有 AudioSource 组件
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 配置 AudioSource
        audioSource.clip = musicClip;
        audioSource.loop = false; // 如果需要循环播放
        audioSource.playOnAwake = false; // 不在启动时自动播放

        // 启动协程
        StartCoroutine(PlayMusicAfterDelay());
    }

    private IEnumerator PlayMusicAfterDelay()
    {
        // 等待指定的延迟时间
        yield return new WaitForSeconds(delay);

        // 播放音乐
        if (audioSource != null && musicClip != null)
        {
            audioSource.Play();
        }
    }
}