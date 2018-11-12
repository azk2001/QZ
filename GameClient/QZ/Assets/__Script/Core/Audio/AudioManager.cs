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

    private Dictionary<int, AudioClip> allAudioDic = new Dictionary<int, AudioClip>();

    public void Play(int id, Vector3 playPosition)
    {
        c_sfx sfx = c_sfx.GetThis(id);
        AudioClip audioClip = null;
        if (allAudioDic.ContainsKey(id) == false)
        {
            audioClip = Resources.Load<AudioClip>(sfx.path);
            allAudioDic[id] = audioClip;
        }
        else
        {
            audioClip = allAudioDic[id];
        }

        AudioSource.PlayClipAtPoint(audioClip, playPosition);
    }

   
}
