using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //create a static variable for SoundManager
    public static SoundManager instance { get; private set; }
    private AudioSource source;

    private void Awake()
    {
        //set up a singleton pattern for SoundManager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
        
        source = GetComponent<AudioSource>();

    }

    // public method to allow other scripts to access PlaySound()
    public void PlaySound(AudioClip sound)
    {
        //plays the sound passed in by other scripts when the SoundManager instance is called through the SoundManager's audio source component
        source.PlayOneShot(sound);
    }

}
