using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    public Image[] scoreImages;
    public Sprite[] fonts;

    public GameObject scoreBar;
    public int score = 0;
    



    public void SetActive(bool value)
    {
        scoreText.gameObject.SetActive(value);
        scoreBar.SetActive(value);
    }
    public void AddPoint(int value)
    {
        score += value;

        string prefix =
            score < 10 ?
            "000" :
            score < 100 ?
            "00" :
            score < 1000 ?
            "0" :
            "";

        // Set score text
        //scoreText.text = prefix + score.ToString();

        // Set score string
        string scoreString = prefix + score.ToString();

        // Translate string to image
        for (int i = 0; i < scoreString.Length; i++)
        {
            // Get the index of the char in the string
            int index = (int)char.GetNumericValue(scoreString[i]);

            // Get the image from the array
            scoreImages[i].sprite = fonts[index];
        }

    }
}