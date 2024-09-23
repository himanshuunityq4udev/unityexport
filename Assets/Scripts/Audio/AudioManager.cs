using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class AudioManager :MonoBehaviour
{
    [SerializeField] AudioSource  musicSource;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] GameObject droneHolder;
    [SerializeField]  Toggle vibration;
  
    private MusicLibrary[] musicLibraries;
    private SoundLibrary[] SoundLibraries;

    private void Awake()
    {
        SoundLibraries = AudioProvider.Instance.SoundLibraries;
        musicLibraries = AudioProvider.Instance.MusicLibraries;
    }

    public void GetMusicAudioSource()
    {
        if (musicSource == null)
        {
            musicSource = droneHolder.transform.GetChild(0).GetComponent<AudioSource>();
        }
    }


  
    public AudioClip GetAudioClipByName(string _audioName)
    {
        foreach (var soundLibrary in SoundLibraries)
        {
            if (soundLibrary.audioName == _audioName)
            {
                int random = Random.Range(0, soundLibrary.audioClips.Length - 1);
                return soundLibrary.audioClips[random];
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


    public void Play2Dsound(string _audioName)
    {
        sfxSource.PlayOneShot(GetAudioClipByName(_audioName));
        if (vibration.isOn)
        {
           // HapticFeedback.MediumFeedback();
        }
    }
}
