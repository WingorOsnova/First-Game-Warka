using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource audS;
    public static SoundManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        audS = GetComponent<AudioSource>();
    }

    public void PlayerSound(AudioClip value)
    {
        audS.volume = Random.Range(0.3f,0.5f);
        audS.pitch = Random.Range(0.9f, 1.1f);
        audS.PlayOneShot(value);
    }
}
