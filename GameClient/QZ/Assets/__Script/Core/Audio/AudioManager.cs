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
		c_audioPlaySet audioSet = c_audioPlaySet.GetThis (id);
		if (audioSet != null)
        {
			c_sfx curSfx = c_sfx.GetThis(audioSet.pathId);

            string path = curSfx.path;
            AudioClip clip = Resources.Load<AudioClip>(path);

			Transform transWave = AudioRoot.instance.spwanPool.GetSpwanPrefab("AudioWave"+audioSet.wave);

			if(transWave!= null)
			{
				transWave.gameObject.SetActive(true);

				Transform transGroup = transWave.Find("AudioGroup"+audioSet.group);

				transGroup.position = playPosition;

				AudioSource audioSource = transGroup.GetComponent<AudioSource>();
				if(audioSource == null)
					audioSource = transGroup.gameObject.AddComponent<AudioSource>();

	            audioSource.loop = isLoop;
	            audioSource.clip = clip;
	            audioSource.Play();
				audioSource.volume = audioSet.value;

				allAudioDic[id] = audioSource;
			}
        }
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
