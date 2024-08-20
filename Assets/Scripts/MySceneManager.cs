using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MySceneManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;  // Reference to the VideoPlayer component
    public string[] videoClips;  // Array of video file paths
    private int currentVideoIndex = 0;  // Track the current video index

    public Button nextButton;  // Reference to the NextButton
    public string sceneToLoadAfterVideos = "MainScene";  // The scene to load after all videos are played

    void Start()
    {
        if (videoClips.Length > 0)
        {
            PlayVideo(currentVideoIndex);
        }

        if (nextButton != null)
        {
            nextButton.onClick.AddListener(OnNextButtonClicked);
        }
    }

    public void OnStartButtonClicked()
    {
        currentVideoIndex = 0;
        PlayVideo(currentVideoIndex);
    }

    public void OnNextButtonClicked()
    {
        if (currentVideoIndex < videoClips.Length - 1)
        {
            currentVideoIndex++;
            PlayVideo(currentVideoIndex);
        }
        else
        {
            SceneManager.LoadScene(sceneToLoadAfterVideos);
        }
    }

    private void PlayVideo(int index)
    {
        if (videoPlayer != null && videoClips.Length > index)
        {
            videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, videoClips[index]);
            videoPlayer.isLooping = true;
            videoPlayer.Play();
        }
    }

    public void OnStartOverButtonClicked()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OnVideoFinished(VideoPlayer vp)
    {
        OnNextButtonClicked();
    }
}
