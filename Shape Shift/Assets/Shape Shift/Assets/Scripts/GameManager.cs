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

    public GameObject startPanel, endPanel, pausedPanel, pauseButton, muteImage, reviveButton;
    public TextMeshProUGUI scoreText, highScoreText, endScoreText, endHighScoreText;

    [HideInInspector]
    public bool gameIsOver = false;

	void Start () {
        StartPanelActivation();
        HighScoreCheck();
        AudioCheck();
	}

    public void Initialize()
    {
        scoreText.enabled = false;
        pauseButton.SetActive(false);
        FindObjectOfType<PlayerScale>().enabled = false;
    }

    public void StartPanelActivation()
    {
        Initialize();
        startPanel.SetActive(true);
        endPanel.SetActive(false);
        pausedPanel.SetActive(false);
    }

    public void EndPanelActivation()
    {
        foreach (GameObject obst in GameObject.FindGameObjectsWithTag("ObstacleHolder"))
            obst.GetComponent<Rigidbody>().Sleep();

        gameIsOver = true;
        FindObjectOfType<AudioManager>().DeathSound();
        FindObjectOfType<PlayerScale>().enabled = false;
        startPanel.SetActive(false);
        endPanel.SetActive(true);
        pausedPanel.SetActive(false);
        scoreText.enabled = false;
        endScoreText.text = scoreText.text;
        pauseButton.SetActive(false);
        HighScoreCheck();
    }

    public void PausedPanelActivation()
    {
        startPanel.SetActive(false);
        endPanel.SetActive(false);
        pausedPanel.SetActive(true);
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
        pauseButton.SetActive(true);
        scoreText.enabled = true;
        startPanel.SetActive(false);
        FindObjectOfType<AudioManager>().ButtonClickSound();
        FindObjectOfType<Spawner>().Spawn();
        FindObjectOfType<PlayerScale>().enabled = true;
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

    public void PauseButton()
    {
        pauseButton.SetActive(false);
        PausedPanelActivation();
        scoreText.enabled = false;
        FindObjectOfType<AudioManager>().StopBackgroundMusic();
        Time.timeScale = 0f;
    }

    public void ResumeButton()
    {
        Time.timeScale = 1f;
        FindObjectOfType<AudioManager>().PlayBackgroundMusic();
        scoreText.enabled = true;
        pauseButton.SetActive(true);
        pausedPanel.SetActive(false);
    }

    public void HomeButton()
    {
        ResumeButton();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Revive()
    {
        //UNCOMMENT THE FOLLOWING LINES IF YOU ENABLED UNITY ADS AT UNITY SERVICES AND REOPENED THE PROJECT!
        //if (FindObjectOfType<AdManager>().unityAds)
        //    FindObjectOfType<AdManager>().ShowUnityRewardVideoAd();       //Shows Unity Reward Video ad
        //else
        FindObjectOfType<AdManager>().ShowAdmobRewardVideo();       //Shows Admob Reward Video ad

        FindObjectOfType<PlayerScale>().enabled = true;
        endPanel.SetActive(false);
        reviveButton.SetActive(false);
        pauseButton.SetActive(true);
        scoreText.enabled = true;

        foreach (GameObject obst in GameObject.FindGameObjectsWithTag("ObstacleHolder"))
            obst.GetComponent<Rigidbody>().AddForce(Vector3.forward * -FindObjectOfType<Spawner>().obstacleSpeed);

        gameIsOver = false;

        FindObjectOfType<Spawner>().TryToSpawn();
        FindObjectOfType<Collision>().Invoke("CanCollideAgain", 1.5f);
    }
}
