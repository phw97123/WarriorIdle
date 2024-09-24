using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager
{
    private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    private AudioMixer _audioMixer; 

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

        _audioMixer = Managers.ResourceManager.Load<AudioMixer>(Define.AUDIOMIXER);
        _bgm.outputAudioMixerGroup = _audioMixer.FindMatchingGroups("BGM")[0];
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
            {
                audioSource.outputAudioMixerGroup = _audioMixer.FindMatchingGroups("EFFECT")[0]; 
                return audioSource;
            }
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
                _audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20); 
                break;
            case Define.AudioType.Effect:
                _audioMixer.SetFloat("EFFECT", Mathf.Log10(volume) * 20);
                break;
        }
    }

    public void Destroy()
    {
        _effects.Clear();
    }
}
