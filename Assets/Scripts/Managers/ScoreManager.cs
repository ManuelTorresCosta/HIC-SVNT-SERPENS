using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Sprite[] fonts;

    [Header("Score UI")]
    public Text scoreText;
    public Image[] scoreDigits;

    [Header("Tale")]
    public Text taleText;
    public Image[] taleDigits;

    [Header("Stone")]
    public Text stoneText;
    public Image[] stoneDigits;

    [Header("Bonus point")]
    public Image bonusPointImage;
    public Image[] bonusPointDigits;

    public GameObject scoreBar;
    
    [Header("Points")]
    public int score = 0;
    public int tales = 0;
    public int stones = 0;
    
    
    public void SetUIActive(bool value)
    {
        // Score
        scoreText.gameObject.SetActive(value);
        foreach (Image scoreDigit in scoreDigits)
            scoreDigit.gameObject.SetActive(value);

        // Tale
        taleText.gameObject.SetActive(value);
        foreach (Image taleDigit in taleDigits)
            taleDigit.gameObject.SetActive(value);

        // Stone
        stoneText.gameObject.SetActive(value);
        foreach (Image stoneDigit in stoneDigits)
            stoneDigit.gameObject.SetActive(value);

        // Bonus point
        bonusPointImage.gameObject.SetActive(value);
        foreach (Image bonusPointDigit in bonusPointDigits)
            bonusPointDigit.gameObject.SetActive(value);

        // Bar
        scoreBar.SetActive(value);
    }
    public void SetBonusPointUIActive(bool value)
    {
        bonusPointImage.gameObject.SetActive(value);

        foreach (Image image in bonusPointDigits)
            image.gameObject.SetActive(value);

    }

    public void AddPoint(int pointValue, TileType.Type tileType)
    {
        // Get the score as a string
        score += pointValue;

        // Update score UI
        UpdateScoreDigits();

        // Update tale UI
        if (tileType == TileType.Type.Tale)
        {
            UpdateTaleDigit();
        }
        // Update stone UI
        else if (tileType == TileType.Type.Stone)
        {
            UpdateStoneDigit();
        }
    }

    private void UpdateScoreDigits()
    {
        string prefix = score < 10 ? "000" : score < 100 ? "00" : score < 1000 ? "0" : "";
        string scoreStr = prefix + score.ToString();

        // Translate string to image
        for (int i = 0; i < scoreStr.Length; i++)
        {
            // Get the index of the char in the string
            int index = (int)char.GetNumericValue(scoreStr[i]);

            // Get the image from the array
            scoreDigits[i].sprite = fonts[index];
        }
    }
    private void UpdateTaleDigit()
    {
        tales++;
        string prefix = tales < 10 ? "0" : "";
        string talesString = prefix + tales.ToString();

        // Translate string to image
        for (int i = 0; i < talesString.Length; i++)
        {
            // Get the index of the char in the string
            int index = (int)char.GetNumericValue(talesString[i]);

            // Get the image from the array
            taleDigits[i].sprite = fonts[index];
        }
    }
    private void UpdateStoneDigit()
    {
        stones++;
        string prefix = stones < 10 ? "0" : "";
        string stonesString = prefix + stones.ToString();

        // Translate string to image
        for (int i = 0; i < stonesString.Length; i++)
        {
            // Get the index of the char in the string
            int index = (int)char.GetNumericValue(stonesString[i]);

            // Get the image from the array
            stoneDigits[i].sprite = fonts[index];
        }
    }
    public void UpdateBonusPointDigits(int rarePointValue)
    {
        string prefix = rarePointValue < 10 ? "0" : "";
        string valueStr = prefix + rarePointValue.ToString();

        for (int i = 0; i < valueStr.Length; i++)
        {
            int index = (int)char.GetNumericValue(valueStr[i]);
            bonusPointDigits[i].sprite = fonts[index];
        }
    }
}