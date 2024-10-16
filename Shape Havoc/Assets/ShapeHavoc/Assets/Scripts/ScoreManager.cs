using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour {

    public TextMeshProUGUI scoreText;
    public Animation textAnim;
  

    [HideInInspector]
    public int score = 0;



    public void IncrementScore()
    {
        if (FindObjectOfType<Player>().gameIsOver == false)       //If the game is not over
            scoreText.text = (++score).ToString();      //Increments the 'scoretext' text as well as the score variable's value and writes it out to the screen
        textAnim.Play();        //Plays the animation attached to the scoreText
    }


}
