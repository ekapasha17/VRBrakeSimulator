
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource sfxSource;


    [Header("Audio Clips")]
    public AudioClip engineStart;
    public AudioClip engineIdle;
    public AudioClip carMovingSlowly;
    public AudioClip explosion;

    public void PlaySFX(AudioClip clip, bool isLooping = false)
    {
        if (sfxSource != null)
        {
            sfxSource.clip = clip;
            sfxSource.loop = isLooping;
            sfxSource.Play();
        }
    }

}
