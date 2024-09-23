using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioProvider : Singleton<AudioProvider>
{

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] Toggle vibration;

    [SerializeField] private SoundLibrary[] soundLibraries;
    [SerializeField] private MusicLibrary[] musicLibraries;

    public SoundLibrary[] SoundLibraries  => soundLibraries;
    public MusicLibrary[] MusicLibraries  => musicLibraries;

    public AudioMixer AudioMixer => audioMixer;
    public Slider MusicSlider => musicSlider;
    public Slider SfxSlider => sfxSlider; 
    public AudioSource MusicSource => musicSource;
    public AudioSource SfxSource => sfxSource;
    public Toggle Vibration => vibration;
}
[System.Serializable]
public struct SoundLibrary
{
    public string audioName;
    public AudioClip audioClips;
}
[System.Serializable]
public struct MusicLibrary
{
    public string trackName;
    public AudioClip audioClip;
}