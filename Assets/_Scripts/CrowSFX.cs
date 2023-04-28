using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowSFX : MonoBehaviour
{
    [SerializeField] private AudioSource audioSrc;

    public void PlayCrowSound()
    {
        audioSrc.volume = Random.Range(0.8f, 1f);
        audioSrc.pitch = Random.Range(0.75f, 0.9f);
        audioSrc.Play();
    }
}
