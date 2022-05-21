using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;
    public AudioClip welcomeNote;
    public AudioClip followInstructions;
    public AudioClip desiredPlanet;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void playClipAtOrigin(AudioClip audioClip,int duration)
    {
        GameObject soundChild = new GameObject();
        soundChild.transform.parent = this.gameObject.transform;
        soundChild.transform.position = GameManager.instance._origin.transform.position;
        AudioSource audioSource = soundChild.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.Play();
        Destroy(soundChild, duration);
    }
}
