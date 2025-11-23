using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using MessagePipe;
using TestTankProject.Runtime.Core.Sounds;
using TestTankProject.Runtime.Utilities.DictionarySerialization;
using UnityEngine;
using UnityEngine.Audio;
using VContainer;

namespace TestTankProject.Runtime.Sounds
{
    public class AudioManager : MonoBehaviour
    {
        private const int InitialAudioSourceCount = 2;

        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private AudioMixerGroup _defaultGroup;
        [SerializeField] private AudioSource _audioSourcePrefab;
        [SerializeField] private AudioClipDictionary _audioClipDictionary;

        private Transform _transform;
        private IDisposable _disposableForSubscriptions;

        private readonly List<AudioSource> _activeSources = new();
        private readonly List<AudioSource> _audioSourcePool = new();

        [Inject]
        private void Initialize(ISubscriber<PlaySoundCommand> playSoundSubscriber)
        {
            _transform = transform;

            for (int i = 0; i < InitialAudioSourceCount; i++)
            {
                AudioSource audioSource = Instantiate(_audioSourcePrefab, _transform);
                audioSource.gameObject.SetActive(false);
                _audioSourcePool.Add(audioSource);
            }

            DisposableBagBuilder bagBuilder = DisposableBag.CreateBuilder();
            playSoundSubscriber.Subscribe(OnPlaySoundCommand).AddTo(bagBuilder);
            _disposableForSubscriptions = bagBuilder.Build();
        }

        private async void OnPlaySoundCommand(PlaySoundCommand playSoundCommand)
        {
            if (playSoundCommand.Delay != 0f)
                await UniTask.WaitForSeconds(playSoundCommand.Delay);
            
            AudioSource idleAudioSource = GetIdleAudioSource();
            idleAudioSource.clip = _audioClipDictionary[playSoundCommand.SoundType];
            idleAudioSource.spatialize = false;
            idleAudioSource.loop = false;
            idleAudioSource.outputAudioMixerGroup = _defaultGroup;
            idleAudioSource.gameObject.SetActive(true);
            idleAudioSource.Play();
        }

        private AudioSource GetIdleAudioSource()
        {
            AudioSource idleAudioSource =
                _audioSourcePool.FirstOrDefault(source => source.gameObject.activeSelf == false);

            if (idleAudioSource == null)
            {
                idleAudioSource = Instantiate(_audioSourcePrefab, _transform);
                idleAudioSource.gameObject.SetActive(false);
                _audioSourcePool.Add(idleAudioSource);
            }

            return idleAudioSource;
        }
    }
}