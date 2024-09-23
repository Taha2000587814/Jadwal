using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    //----------------------------------------------
    //Thank you for purchasing the asset! If you have any questions/suggestions, don't hesitate to contact me!
    //E-mail: ragendom@gmail.com
    //Please let me know your impressions about the asset by leaving a review, I will appreciate it.
    //----------------------------------------------

    public GameObject startPanel, endPanel, muteImage;
    public TextMeshProUGUI scoreText, highScoreText, endScoreText, endHighScoreText;
    [HideInInspector] public bool gameIsOver = false;

	void Start () {
        //UNCOMMENT THE FOLLOWING LINES IF YOU ENABLED UNITY ADS AT UNITY SERVICES AND REOPENED THE PROJECT!
        //if (FindObjectOfType<AdManager>().unityAds)
        //    CallUnityAds();     //Calls Unity Ads
        //else
        CallAdmobAds();     //Calls Admob Ads

        StartPanelActivation();
        Initialize();
        HighScoreCheck();
        AudioCheck();
	}

    //UNCOMMENT THE FOLLOWING LINES IF YOU ENABLED UNITY ADS AT UNITY SERVICES AND REOPENED THE PROJECT!
    //public void CallUnityAds()
    //{
    //    if (Time.time != Time.timeSinceLevelLoad)
    //        FindObjectOfType<AdManager>().ShowUnityVideoAd();      //Shows Interstitial Ad when game starts (except for the first time)
    //    FindObjectOfType<AdManager>().HideAdmobBanner();
    //}

    public void CallAdmobAds()
    {
        FindObjectOfType<AdManager>().ShowAdmobBanner();        //Shows Banner Ad when game starts
        if (Time.time != Time.timeSinceLevelLoad)
            FindObjectOfType<AdManager>().ShowAdmobInterstitial();      //Shows Interstitial Ad when game starts (except for the first time)
    }

    public void Initialize()
    {
        scoreText.enabled = false;
        FindObjectOfType<FollowPath>().enabled = false;
        FindObjectOfType<Spawner>().enabled = false;
    }

    public void StartPanelActivation()
    {
        startPanel.SetActive(true);
        endPanel.SetActive(false);
    }

    public void EndPanelActivation()
    {
        gameIsOver = true;
        FindObjectOfType<AudioManager>().DeathSound();
        GameObject.FindGameObjectWithTag("Player").GetComponent<Renderer>().enabled = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<ParticleSystem>().Play();
        startPanel.SetActive(false);
        endPanel.SetActive(true);
        scoreText.enabled = false;
        endScoreText.text = scoreText.text;
        HighScoreCheck();
    }

    public void HighScoreCheck()
    {
        if (FindObjectOfType<ScoreManager>().score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", FindObjectOfType<ScoreManager>().score);
        }
        highScoreText.text = "BEST " + PlayerPrefs.GetInt("HighScore", 0).ToString();
        endHighScoreText.text = "BEST " + PlayerPrefs.GetInt("HighScore", 0).ToString();
    }

    public void AudioCheck()
    {
        if (PlayerPrefs.GetInt("Audio", 0) == 0)
        {
            muteImage.SetActive(false);
            FindObjectOfType<AudioManager>().soundIsOn = true;
            FindObjectOfType<AudioManager>().PlayBackgroundMusic();
        }
        else
        {
            muteImage.SetActive(true);
            FindObjectOfType<AudioManager>().soundIsOn = false;
            FindObjectOfType<AudioManager>().StopBackgroundMusic();
        }
    }

    public void StartButton()
    {
        scoreText.enabled = true;
        startPanel.SetActive(false);
        FindObjectOfType<AudioManager>().ButtonClickSound();
        FindObjectOfType<FollowPath>().enabled = true;
        FindObjectOfType<Spawner>().enabled = true;
    }

    public void RestartButton()
    {
        FindObjectOfType<AudioManager>().ButtonClickSound();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AudioButton()
    {
        FindObjectOfType<AudioManager>().ButtonClickSound();
        if (PlayerPrefs.GetInt("Audio", 0) == 0)
            PlayerPrefs.SetInt("Audio", 1);
        else
            PlayerPrefs.SetInt("Audio", 0);
        AudioCheck();
    }
}
