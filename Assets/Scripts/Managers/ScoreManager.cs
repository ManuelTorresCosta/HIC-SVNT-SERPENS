using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
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

        scoreText.text = prefix + score.ToString();
    }
}