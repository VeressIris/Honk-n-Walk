using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowSFX : MonoBehaviour
{
    private AudioSource audioSrc;

    void Start()
    {
        audioSrc = GetComponent<AudioSource>();    
    }

    public void PlaySound()
    {
        audioSrc.volume = Random.Range(0.8f, 1f);
        audioSrc.pitch = Random.Range(0.75f, 0.9f);
        audioSrc.Play();
    }
}
