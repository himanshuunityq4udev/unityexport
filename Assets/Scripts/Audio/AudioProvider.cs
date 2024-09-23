using UnityEngine;

public class AudioProvider : Singleton<AudioProvider>
{
    [SerializeField] private SoundLibrary[] soundLibraries;
    [SerializeField] private MusicLibrary[] musicLibraries;

    public SoundLibrary[] SoundLibraries  => soundLibraries;
    public MusicLibrary[] MusicLibraries  => musicLibraries;

}
[System.Serializable]
public struct SoundLibrary
{
    public string audioName;
    public AudioClip[] audioClips;
}
[System.Serializable]
public struct MusicLibrary
{
    public string trackName;
    public AudioClip audioClip;
}