using UnityEngine;
using UnityEngine.Audio;

namespace Core
{
    public class AudioManager : Singleton<AudioManager>
    { 
        [SerializeField] private AudioMixer audioMixer;
        public AudioMixer AudioMixer { get => audioMixer; set => audioMixer = value; }
    }
}