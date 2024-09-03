using UnityEngine;

namespace RDC
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : Singleton<SoundManager>
    {
        [SerializeField] private AudioClip droneSoundClip;
        //  [SerializeField] private float maxVolume = 0.5f;
        [SerializeField] private float maxPitch = 2.0f;
        [SerializeField] private float defaultVolume = 0.5f;
        [SerializeField] private float defaultPitch = 1.0f;

        private AudioSource droneAudioSource;
        public AudioSource DroneAudioSource => droneAudioSource;

        protected override void Awake()
        {
            base.Awake();
            droneAudioSource = GetComponent<AudioSource>();
            droneAudioSource.clip = droneSoundClip;
            droneAudioSource.Play();
        }

        public void DroneSound()
        {

            bool isThrottleActive = InputManager.Instance.Throttle == 1 || InputManager.Instance.Throttle == -1;
            bool isCyclicActive = false;

            foreach (var direction in Direction.Directions)
            {
                if (InputManager.Instance.Cyclic == direction)
                {
                    isCyclicActive = true;
                    break;
                }
            }

            if (isThrottleActive || isCyclicActive)
            {
                // Increase volume and pitch
                //  droneAudioSource.volume = Mathf.Min(droneAudioSource.volume + Time.deltaTime, maxVolume);
                droneAudioSource.pitch = Mathf.Min(droneAudioSource.pitch + Time.deltaTime, maxPitch);
            }
            else
            {
                // Return to default volume and pitch
                //droneAudioSource.volume = Mathf.Max(droneAudioSource.volume - Time.deltaTime, defaultVolume);
                droneAudioSource.pitch = Mathf.Max(droneAudioSource.pitch - Time.deltaTime, defaultPitch);
            }
        }
    }
}