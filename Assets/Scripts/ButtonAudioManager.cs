using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonAudioManager : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public AudioClip hoverClip; // 悬停时播放的音频
    public AudioClip clickClip; // 点击时播放的音频
    private AudioSource audioSource;

    void Start()
    {
        // 确保 AudioSource 组件存在
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // 当鼠标悬停在按钮上时触发
    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayAudio(hoverClip);
    }

    // 当按钮被点击时触发
    public void OnPointerClick(PointerEventData eventData)
    {
        PlayAudio(clickClip);
    }

    // 播放指定音频
    private void PlayAudio(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
