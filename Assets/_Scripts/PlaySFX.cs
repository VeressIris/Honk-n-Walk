using UnityEngine;

public class PlaySFX : MonoBehaviour
{
    public AudioSource audioSrc;

    public void PlaySound(AudioClip clip, float volume)
    {
        audioSrc.PlayOneShot(clip, volume);
    }
}
