using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    //------------------------CREDITS----------------------------
    //Background music by Eric Matyas: http://www.soundimage.org
    //Sound effects: https://www.noiseforfun.com
    //-----------------------------------------------------------

    [SerializeField]
    private AudioSource backgroundMusic, deathSound, scoreSound, havocSound;

    [HideInInspector]
    public bool soundIsOn = true;       //GameManager script might modify this value

    //Functions are called when it is necessary

    public void StopBackgroundMusic()
    {
        backgroundMusic.Stop();
    }

    public void PlayBackgroundMusic()
    {
        if (soundIsOn)
            backgroundMusic.Play();
    }

    public void ScoreSound()
    {
        if (soundIsOn)
            scoreSound.Play();
    }

    public void DeathSound()
    {
        if (soundIsOn)
            deathSound.Play();
    }

    public void HavocSound()
    {
        if (soundIsOn)
            havocSound.Play();
    }
}
