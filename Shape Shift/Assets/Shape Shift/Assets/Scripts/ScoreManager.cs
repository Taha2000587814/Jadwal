using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour {

    public TextMeshProUGUI scoreText, tokenText;

    private Animation scoreTextAnim, tokenTextAnim, cameraAnim;

    [HideInInspector]
    public int score = 0;

    void Start()
    {
        cameraAnim = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animation>();     //Initializes cameraAnim
        scoreTextAnim = scoreText.gameObject.GetComponent<Animation>();     //Initializes socreTextAnim
        tokenTextAnim = tokenText.gameObject.GetComponent<Animation>();     //Initializes tokenTextAnim
        tokenText.text = PlayerPrefs.GetInt("Token", 0).ToString();     //Writes out the number of tokens to the screen
    }

    public void IncrementScore()
    {
        if (FindObjectOfType<GameManager>().gameIsOver == false)       //If the game is not over
            scoreText.text = (++score).ToString();      //Increments the 'scoretext' text as well as the score variable's value and writes it out to the screen
        scoreTextAnim.Play();       //Plays scoreTextAnim
        FindObjectOfType<AudioManager>().ScoreSound();      //Plays scoreSound
        cameraAnim.Play();      //Plays camera animation
    }

    public void IncrementScoreContinously()
    {
        if (FindObjectOfType<GameManager>().gameIsOver == false)       //If the game is not over
        {
            Invoke("IncrementScoreContinously", 1f);        //Increments score again after x secs
            scoreText.text = (++score).ToString();      //Increments the 'scoretext' text as well as the score variable's value and writes it out to the screen
            //scoreTextAnim.Play();       //Plays scoreTextAnim
        }
    }

    public void IncrementToken()
    {
        if (FindObjectOfType<GameManager>().gameIsOver == false)       //If the game is not over
        {
            PlayerPrefs.SetInt("Token", PlayerPrefs.GetInt("Token", 0) + 1);        //Increases the number of tokens
            tokenText.text = PlayerPrefs.GetInt("Token", 0).ToString();     //Writes out the number of tokens to the screen
            tokenTextAnim.Play();       //Plays tokenTextAnim
            FindObjectOfType<AudioManager>().TokenSound();      //Plays tokenSound
        }
    }

    public void IncrementToken(int countOfToken)
    {
        if (FindObjectOfType<GameManager>().gameIsOver == false)       //If the game is not over
        {
            PlayerPrefs.SetInt("Token", PlayerPrefs.GetInt("Token", 0) + countOfToken);        //Increases the number of tokens
            tokenText.text = PlayerPrefs.GetInt("Token", 0).ToString();     //Writes out the number of tokens to the screen
            tokenTextAnim.Play();       //Plays tokenTextAnim
            FindObjectOfType<AudioManager>().TokenSound();      //Plays tokenSound
        }
    }

    public void DecrementToken(int decreaseValue)
    {
        PlayerPrefs.SetInt("Token", PlayerPrefs.GetInt("Token", 0) - decreaseValue);        //Decreases the number of tokens by decreaseValue
        tokenText.text = PlayerPrefs.GetInt("Token", 0).ToString();     //Writes out the number of tokens to the screen
    }
}
