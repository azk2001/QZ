using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager
{
    private static AudioManager _instance = null;
    public static AudioManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AudioManager();
            }
            return _instance;
        }
    }

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

    private float _backVolume = 1;
    public void Play(int id, Vector3 playPosition, bool isLoop = false)
    {
	
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
		foreach (var audio in allAudioDic) {
			audio.Value.Stop();
		}
	}
}
