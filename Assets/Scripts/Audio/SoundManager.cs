using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public enum SoundType
{
    Background,
    
}
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour,ISoundManager
{
    [SerializeField]private AudioMixer audioMixer;

    [SerializeField]  private AudioClip EntryClip;
     [SerializeField] private AudioClip ExitClip;

     [SerializeField] private AudioSource sfxaudioSource;
     [SerializeField] private AudioSource musicAudioSource;
     [SerializeField] private Coroutine AudioCoroutine;

     [SerializeField] private Slider musicSlider;
     [SerializeField] private Slider sfxSlider;

    Toggle vibration;

    private void Awake()
    {
        audioMixer = AudioProvider.Instance.AudioMixer;
        sfxaudioSource = AudioProvider.Instance.SfxSource;
        musicAudioSource = AudioProvider.Instance.MusicSource;
        EntryClip = AudioProvider.Instance.SoundLibraries[0].audioClips;
        ExitClip = AudioProvider.Instance.SoundLibraries[1].audioClips;

        musicSlider = AudioProvider.Instance.MusicSlider;
        sfxSlider = AudioProvider.Instance.SfxSlider;

        vibration = AudioProvider.Instance.Vibration;

        sfxaudioSource.playOnAwake = false;
        sfxaudioSource.loop = false;
        sfxaudioSource.spatialBlend = 0;
        sfxaudioSource.enabled = false;

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
            DiscardVolume();
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

    public void EnterPlayClip(bool PlayAudio)
    {
        PlayEntryClip(PlayAudio);
    }

    public void ExitPlayClip(bool PlayAudio)
    {
        PlayExitClip(PlayAudio);
    }

    private void PlayEntryClip(bool PlayAudio)
    {
        if (PlayAudio && EntryClip != null && sfxaudioSource != null)
        {
            if (AudioCoroutine != null)
            {
                StopCoroutine(AudioCoroutine);
            }

            AudioCoroutine = StartCoroutine(PlayClip(EntryClip));
        }
    }

    private void PlayExitClip(bool PlayAudio)
    {
        if (PlayAudio && ExitClip != null && sfxaudioSource != null)
        {
            if (AudioCoroutine != null)
            {
                StopCoroutine(AudioCoroutine);
            }

            AudioCoroutine = StartCoroutine(PlayClip(ExitClip));
        }
    }

    private IEnumerator PlayClip(AudioClip Clip)
    {
        sfxaudioSource.enabled = true;

        WaitForSeconds Wait = new WaitForSeconds(Clip.length);

        sfxaudioSource.PlayOneShot(Clip);

        yield return Wait;

        //sfxaudioSource.enabled = false;
    }


    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        //PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void SetSFXVolume()
    {
        float sfxVolume = sfxSlider.value;
        audioMixer.SetFloat("sfx", Mathf.Log10(sfxVolume) * 20);
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


    public void DiscardVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        SetMusicVolume();
        SetSFXVolume();
    }


   /* public AudioClip GetAudioClipByName(string _audioName)
    {
        foreach (var soundLibrary in SoundLibraries)
        {
            if (soundLibrary.audioName == _audioName)
            {
                return soundLibrary.audioClips;
            }
        }
        return null;
    }*/
   /* public AudioClip GetMusicTrackByName(string _trackName)
    {
        foreach (var track in musicLibraries)
        {
            if (track.trackName == _trackName)
            {
                return track.audioClip;
            }
        }
        return null;
    }*/


    public void DiscardVibration()
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

       // sfxSource.PlayOneShot(GetAudioClipByName(_audioName));
        if (vibration.isOn)
        {
            // HapticFeedback.MediumFeedback();
        }
    }

}
