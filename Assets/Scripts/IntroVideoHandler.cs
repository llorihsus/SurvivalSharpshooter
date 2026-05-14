using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroVideoHandler : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    void Start()
    {   
        
    }

    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        // Subscribe to the event that triggers when the video reaches the end
        videoPlayer.Prepare();
        videoPlayer.time = 0;
        videoPlayer.loopPointReached += OnVideoEnd;
        videoPlayer.prepareCompleted += PlayVideo;
    }

    void PlayVideo(VideoPlayer vp)
    {
        vp.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        // Replace "MenuScene" with the name or index of your target scene
        SceneManager.LoadScene("Main Menu");
    }
}
