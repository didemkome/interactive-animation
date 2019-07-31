using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class VideoPlayer : MonoBehaviour
{
    public static string VideoURL;
    public GameObject tv;
    public static bool isPlaying = false;
    public UnityEngine.Video.VideoPlayer videoPlayer;
    private AudioSource audioSource;
    
    // Use this for initialization
    void Start()
    {
        VideoURL = "empty";
        audioSource = gameObject.AddComponent<AudioSource>();
        //VideoPlayer vPlayer = new VideoPlayer();
        //Disable Play on Awake for both Video and Audio
        videoPlayer.playOnAwake = false;
        audioSource.playOnAwake = false;
        audioSource.Pause();
        videoPlayer.source = VideoSource.Url;
    }

    public void PlayVideo(string Url)
    {
        videoPlayer.url = Url;
        Debug.Log(videoPlayer.url);
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        //Assign the Audio from Video to AudioSource to be played
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);
        //Set video To Play then prepare Audio to prevent Buffering        
        videoPlayer.Prepare();
        videoPlayer.Play();
        isPlaying = videoPlayer.isPlaying;
        audioSource.Play();
    }
    // Update is called once per frame
    void Update()
    {
        if (VideoURL != "empty")
        {
            PlayVideo(VideoURL);
            VideoURL = "empty";
        }
    }
}