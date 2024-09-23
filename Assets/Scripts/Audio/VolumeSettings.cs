using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
//using CandyCoded.HapticFeedback;
public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] Toggle vibration;
   // [SerializeField] GameObject droneHolder;

    private MusicLibrary[] musicLibraries;
    private SoundLibrary[] SoundLibraries;


    private void Awake()
    {
        SoundLibraries = AudioProvider.Instance.SoundLibraries;

        musicLibraries = AudioProvider.Instance.MusicLibraries;

        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            PlayerPrefs.SetFloat("sfxVolume", 1);
            PlayerPrefs.SetInt("Vibration", 1);
        }
      
    }
    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
        }

        if (PlayerPrefs.HasKey("Vibration"))
        {
            if (PlayerPrefs.GetInt("Vibration") == 0)
            {
                vibration.isOn = false;
            }
            else
            {
                vibration.isOn = true;
            }
        }
    }


    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("music", Mathf.Log10(volume)*20);
        //PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void SetSFXVolume()
    {
        float sfxVolume = sfxSlider.value;
        audioMixer.SetFloat("sfx", Mathf.Log10(sfxVolume)*20);
        //PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
    }

    public void SaveVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
        float sfxVolume = sfxSlider.value;
        audioMixer.SetFloat("sfx", Mathf.Log10(sfxVolume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
    }


    public void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        SetMusicVolume();
        SetSFXVolume();
    }


   /* public void GetMusicAudioSource()
    {
        if (musicSource == null)
        {
            musicSource = droneHolder.transform.GetChild(0).GetComponent<AudioSource>();
        }
    }*/

    public AudioClip GetAudioClipByName(string _audioName)
    {
        foreach (var soundLibrary in SoundLibraries)
        {
            if (soundLibrary.audioName == _audioName)
            {
                return soundLibrary.audioClips;
            }
        }
        return null;
    }
    public AudioClip GetMusicTrackByName(string _trackName)
    {
        foreach (var track in musicLibraries)
        {
            if (track.trackName == _trackName)
            {
                return track.audioClip;
            }
        }
        return null;
    }


    public void SaveVibration()
    {

        if (PlayerPrefs.GetInt("Vibration") == 1)
        {
            vibration.isOn = true;
        }
        else
        {
            vibration.isOn = false;
        }
    }

    public void SaveplayerVibration()
    {

        if (PlayerPrefs.GetInt("Vibration") == 1)
        {
            PlayerPrefs.SetInt("Vibration", 0);
        }
        else
        {
            PlayerPrefs.SetInt("Vibration", 1);
        }
    }

    public void Play2Dsound(string _audioName)
    {
      
        sfxSource.PlayOneShot(GetAudioClipByName(_audioName));
        if (vibration.isOn)
        {
           // HapticFeedback.MediumFeedback();
        }
    }
}
