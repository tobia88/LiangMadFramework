using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


[System.Serializable]
public class Sound {
    public AudioClip clip;
    [Range(0,1)]
    public float volume = 1f;
    public float pitch = 1f;
    public bool randomPitch = false;
    public bool loop = false;
    public float minPitch = 0.995f;
    public float maxPitch = 1.05f;

    public float GetPitch() {
        return ( randomPitch ) ? Random.Range(minPitch, maxPitch) : pitch;
    }
}

public class AudioMng : MonoBehaviour {
    private static AudioMng _Instance;

    public static AudioMng Instance {
        get { return _Instance; }
    }

    public static void Init() {
        _Instance = FindObjectOfType<AudioMng>();
    }

    public Dictionary<AudioClip, AudioSource> sources = new Dictionary<AudioClip, AudioSource>();

    public void Play(Sound p_snd, bool p_forceReplay = false) {
        AudioSource src = GetSource(p_snd);

        if (src.isPlaying && !p_forceReplay)
            return;

        src.volume = p_snd.volume;
        src.pitch = p_snd.GetPitch();
        src.loop = p_snd.loop;
        src.Play();
    }

    public void PlayOneShot(Sound p_snd) {
        AudioSource src = GetSource(p_snd);

        src.pitch = p_snd.GetPitch();
        src.PlayOneShot(p_snd.clip, src.volume);
    }

    public void Fade(Sound p_snd, float p_volume, float p_duration) {
        AudioSource src = GetSource(p_snd);
        src.DOFade(p_volume, p_duration);
    }

    public void Stop(Sound p_snd) {
        if (sources.ContainsKey(p_snd.clip))
            sources[p_snd.clip].Stop();
    }

    public AudioSource Add(Sound p_snd) {
        GameObject go = new GameObject(p_snd.clip.name);
        go.transform.SetParent(transform);

        AudioSource retval = go.AddComponent<AudioSource>();
        sources.Add(p_snd.clip, retval);
        retval.clip = p_snd.clip;
        return retval;
    }

    public AudioSource GetSource(Sound p_snd) {
        AudioSource src = null;
        if (!sources.ContainsKey(p_snd.clip))
            src = Add(p_snd);
        else
            src = sources[p_snd.clip];

        return src;
    }
}
