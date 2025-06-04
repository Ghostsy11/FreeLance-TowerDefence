using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    [SerializeField] List<AudioClip> sources;
    [SerializeField] AudioSource backgroundMusic;
    [SerializeField] float elapsedTime;

    void Start()
    {
        AudioManager.instance = this;
        backgroundMusic = GetComponent<AudioSource>();
        backgroundMusic.clip = sources[GetRandomSong()];
        backgroundMusic.Play();

    }

    private void Update()
    {
        TimeToChangeSong();
        elapsedTime += Time.deltaTime;
    }

    private int GetRandomSong()
    {
        return Random.Range(0, sources.Count);
    }

    private void TimeToChangeSong()
    {
        float timeToReset = 15f;

        if (elapsedTime >= timeToReset)
        {
            var musicIndex = GetRandomSong();
            backgroundMusic.Stop();
            backgroundMusic.clip = sources[musicIndex];
            backgroundMusic.Play();
            elapsedTime = 0;

        }


    }

}
