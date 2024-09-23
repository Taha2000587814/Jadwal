using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    //------------------------CREDITS----------------------------
    //Background music by Eric Matyas: http://www.soundimage.org
    //Sound effects: https://www.noiseforfun.com
    //-----------------------------------------------------------

    [SerializeField]
    private AudioSource backgroundMusic, tokenSound, scoreSound, deathSound, buttonClickSound, skinSwitchSound;

    [HideInInspector]
    public bool soundIsOn = true;       //GameManager script might modify this value

    //Functions are called by other scripts when it is necessary

    public void StopBackgroundMusic()
    {
        backgroundMusic.Stop();
    }

    public void PlayBackgroundMusic()
    {
        if (soundIsOn)
            backgroundMusic.Play();
    }

    public void TokenSound()
    {
        if(soundIsOn)
            tokenSound.Play();
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

    public void ButtonClickSound()
    {
        if (soundIsOn)
            buttonClickSound.Play();
    }

    public void SkinSwitchSound()
    {
        if (soundIsOn)
            skinSwitchSound.Play();
    }
}
