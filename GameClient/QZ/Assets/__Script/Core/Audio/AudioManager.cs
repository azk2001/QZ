using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingleClass<AudioManager>
{
    private float _backVolume = 1;
    public float BackVolume
    {
        get
        {
            return _backVolume;
        }
        set
        {
            _backVolume = Mathf.Clamp01(value);
        }
    }

    private Dictionary<int, AudioSource> allAudioDic = new Dictionary<int, AudioSource>();

    public void Play(int id, Vector3 playPosition)
    {
        c_sfx sfx = c_sfx.GetThis(id);
        AudioSource audioSource = null;
        if (allAudioDic.ContainsKey(id) == false)
        {
            audioSource = Resources.Load<AudioSource>(sfx.fileName);
            allAudioDic[id] = audioSource;
        }
        else
        {
            audioSource = allAudioDic[id];
        }

        AudioSource.PlayClipAtPoint(audioSource.clip, playPosition);
    }

    public void Stop(int id)
    {
        if (allAudioDic.ContainsKey(id) == true)
        {
            allAudioDic[id].Stop();
        }
    }

    public void StopAllAudio()
    {
        foreach (var audio in allAudioDic)
        {
            audio.Value.Stop();
        }
    }
}
