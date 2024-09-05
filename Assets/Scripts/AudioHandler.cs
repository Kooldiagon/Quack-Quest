using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioHandler : SingletonMonoBehaviour<AudioHandler>
{
    [SerializeField] private AudioMixerGroup music, sound;
    [SerializeField] private AudioSource musicSource;
    private List<AudioSource> audioSources;

    private void Start()
    {
        AdjustMusicVolume(GameManager.Instance.SaveData.MusicVolume);
        AdjustSoundVolume(GameManager.Instance.SaveData.SoundVolume);
        audioSources = new List<AudioSource>();
        StartCoroutine(CleanUpCheck());
    }

    private IEnumerator CleanUpCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(60f);
            foreach (AudioSource source in new List<AudioSource>(audioSources))
            {
                if (!source.isPlaying)
                {
                    Destroy(source);
                    audioSources.Remove(source);
                }
            }
        }
    }

    public void AdjustMusicVolume(int value)
    {
        music.audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp((float)value / 100f, 0.0001f, 1f)) * 20f);
    }

    public void AdjustSoundVolume(int value)
    {

        sound.audioMixer.SetFloat("SoundVolume", Mathf.Log10(Mathf.Clamp((float)value / 100f, 0.0001f, 1f)) * 20f);
    }

    public void PlaySound(AudioClip audio)
    {
        AudioSource sourceToUse = null;
        foreach (AudioSource source in audioSources)
        {
            if(!source.isPlaying)
            {
                sourceToUse = source;
                break;
            }
        }

        if (sourceToUse == null)
        {
            sourceToUse = gameObject.AddComponent<AudioSource>();
            sourceToUse.outputAudioMixerGroup = sound;
            audioSources.Add(sourceToUse);
        }

        sourceToUse.clip = audio;
        sourceToUse.Play();
    }
}
