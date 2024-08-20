using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Button nextButton;
    public VideoClip[] videoClips;  // 视频片段数组
    private int currentVideoIndex = 0;
    public SpriteRenderer spriteRenderer;  // Reference to the SpriteRenderer component
    public float fadeDuration = 1.0f;  // Duration of the fade effect
    public float fadeDurationBetweenVideos = 3.0f;  // Duration of the fade effect

    void Start()
    {
        // 初始化按钮
        nextButton.onClick.AddListener(OnNextButtonClicked);

        // 播放第一个视频
        PlayVideo(currentVideoIndex);
    }

    void PlayVideo(int index)
    {
        if (index < videoClips.Length)
        {
            videoPlayer.clip = videoClips[index];
            videoPlayer.Play();
        }
    }

    void OnNextButtonClicked()
    {
        currentVideoIndex++;
        if (currentVideoIndex < videoClips.Length)
        {
            StartCoroutine(SwitchVideoCoroutine());
        }
        else
        {
            // 如果所有视频播放完毕，跳转到主场景
            StartCoroutine(FadeAndLoadScene("MainScene"));
        }
    }
    
    private IEnumerator SwitchVideoCoroutine()
    {
        // 先等待当前视频的淡出
        yield return StartCoroutine(FadeOutCurrentVideo());

        // 播放下一个视频
    }

    private IEnumerator FadeOutCurrentVideo()
    {
        videoPlayer.Stop();
        PlayVideo(currentVideoIndex);
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            float elapsedTime = 0f;

            // Fade out the current video
            while (elapsedTime < fadeDurationBetweenVideos)
            {
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Clamp01(1 - (elapsedTime / fadeDurationBetweenVideos));
                spriteRenderer.color = color;
                yield return null;
            }

            // Ensure transparency is set to 0%
            color.a = 0f;
            spriteRenderer.color = color;
        }
        // Optionally, you might want to stop the video playback while waiting
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            float elapsedTime = 0f;

            // Set initial transparency to 0
            color.a = 0f;
            spriteRenderer.color = color;

            // Gradually increase transparency to 100%
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
                spriteRenderer.color = color;
                yield return null;
            }

            // Ensure transparency is set to 100%
            color.a = 1f;
            spriteRenderer.color = color;
        }

        // Load the MainScene
        SceneManager.LoadScene(sceneName);
    }
}
