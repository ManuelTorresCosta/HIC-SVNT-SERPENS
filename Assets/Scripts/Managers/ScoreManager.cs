using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Sprite[] fonts;

    [Header("Score UI")]
    public Image[] scoreDigits;

    [Header("Rare point")]
    public Image rarePoint;
    public Image[] rarePointDigits;

    public GameObject scoreBar;
    public int score = 0;
    

    public void SetUIActive(bool value)
    {
        foreach (Image image in scoreDigits)
            image.gameObject.SetActive(value);

        rarePoint.gameObject.SetActive(false);

        foreach (Image image in rarePointDigits)
            image.gameObject.SetActive(value);

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
            scoreDigits[i].sprite = fonts[index];
        }

    }

    public void SetRarePointUIActive(bool value)
    {
        rarePoint.gameObject.SetActive(value);

        foreach (Image image in rarePointDigits)
            image.gameObject.SetActive(value);

    }
    public void UpdateRarePointUI(int rarePointValue)
    {
        string valueStr = rarePointValue.ToString();
        for (int i = 0; i < valueStr.Length; i++)
        {
            int index = (int)char.GetNumericValue(valueStr[i]);
            rarePointDigits[i].sprite = fonts[index];
        }
    }
}