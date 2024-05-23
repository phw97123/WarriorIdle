using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SoundManager
{
    private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    private AudioSource _bgm;
    private List<AudioSource> _effects = new List<AudioSource>();

    private GameObject _sound;
    private GameObject _effectGO;

    public void Init()
    {
        _sound = new GameObject() { name = "@Sound" };
        _bgm = _sound.AddComponent<AudioSource>();
        
        _effectGO = new GameObject() { name = "effect" };
        _effectGO.transform.SetParent(_sound.transform, false);
    }

    private AudioClip LoadAudioClip(string name)
    {
        if (!_audioClips.ContainsKey(name))
        {
            AudioClip clip = Managers.ResourceManager.Load<AudioClip>(name);
            if (clip == null)
            {
                Debug.LogError($"AudioClip 로드 실패 : {name}");
                return null;
            }
            _audioClips.Add(name, clip);
        }
        return _audioClips[name];
    }

    public void Play(string name, Define.AudioType type = Define.AudioType.Effect)
    {
        AudioClip clip = LoadAudioClip(name);

        switch (type)
        {
            case Define.AudioType.Bgm:
                if (_bgm.isPlaying) _bgm.Stop();
                _bgm.clip = clip;
                _bgm.loop = true;
                _bgm.Play();
                break; 
            case Define.AudioType.Effect:
                AudioSource effect = GetEffectAudioSource();
                effect.clip = clip;
                effect.Play(); 
                break;
        }
    }

    private AudioSource GetEffectAudioSource()
    {
        foreach (var audioSource in _effects)
        {
            if (!audioSource.isPlaying)
                return audioSource;
        }

        AudioSource newAudioSource = _effectGO.AddComponent<AudioSource>();
        _effects.Add(newAudioSource);
        return newAudioSource;
    }

    public void Stop(Define.AudioType type)
    {
        switch (type)
        {
            case Define.AudioType.Bgm:
                if(_bgm.isPlaying)
                    _bgm.Stop();
                break;
            case Define.AudioType.Effect:
                foreach (var audioSouce in _effects)
                {
                   if(audioSouce.isPlaying)
                        audioSouce.Stop();
                }
                break;
        }
    }

    public void SetVolume(float volume, Define.AudioType type)
    {
        switch (type)
        {
            case Define.AudioType.Bgm:
                _bgm.volume = volume;
                break;
            case Define.AudioType.Effect:
                foreach (var audioSouce in _effects)
                {
                    audioSouce.volume = volume;
                }
                break;
        }
    }
}
