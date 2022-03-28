using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource audioSource;
    public AudioSource bgAudioSource;

    [SerializeField]
    private AudioClip clickSnd;
    [SerializeField]
    private AudioClip breakSnd;

    //Creating instance of class
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        
        ManageBgSound();
    }

    #region Manage Background Sound
    //Check sound is off or not
    internal void ManageBgSound()
    {
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetString("Sound").Equals("Off"))
                bgAudioSource.Stop();
            else
                bgAudioSource.Play();
        }
        else
        {
            PlayerPrefs.SetString("Sound", "On");
        }

        if (!PlayerPrefs.HasKey("Sfx"))
        {
            PlayerPrefs.SetString("Sfx", "On");
        }
    }
    #endregion

    #region Plat Sfx sounds

    //Play click sound
    internal void PlayClickSound()
    {
        if (PlayerPrefs.GetString("Sfx").Equals("Off"))
            return;
        audioSource.Stop();
        audioSource.PlayOneShot(clickSnd);
    }

    //Play Brick break sound
    internal void PlayBreakSound()
    {
        if (PlayerPrefs.GetString("Sfx").Equals("Off"))
            return;
        audioSource.Stop();
        audioSource.PlayOneShot(breakSnd);
    }
    #endregion

}
