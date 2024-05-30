using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientAudio : MonoBehaviour
{
    public AudioSource ambientSound;
    public AudioClip[] randomAudioClips;

    private float nextRandomTime;
    private float randomInterval = 60f;

    private void Start()
    {
        nextRandomTime = Time.time + randomInterval;
    }

    private void Update()
    {
        if (Time.time >= nextRandomTime)
        {
            PlayRandomAudio();
            nextRandomTime = Time.time + randomInterval;
        }
    }

    private void PlayRandomAudio()
    {
        int randomIndex = Random.Range(0, randomAudioClips.Length);
        ambientSound.clip = randomAudioClips[randomIndex];
        ambientSound.Play();
    }
}
