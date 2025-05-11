using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundInstance : MonoBehaviour
{

    AudioSource audioSource;
    public AudioClip audioclip;
    public bool loop;
    public float Volume;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlaySound();
    }

    // Update is called once per frame
    void Update()
    {
        if (audioclip != null)
        {
            if (audioSource.isPlaying == false && !loop)
            {
                Destroy(gameObject);
            }
        }
    }

    public void PlaySound()
    {
        audioSource.clip = audioclip;
        audioSource.loop = loop;
        audioSource.volume = Volume;
        audioSource.Play();
    }
}
